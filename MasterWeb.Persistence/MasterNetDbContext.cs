using Bogus;
using MasterWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace MasterWeb.Persistence
{
    public class MasterNetDbContext : DbContext
    {
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Instructor> Instructores { get; set; }
        public DbSet<Precio> Precios { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Data Source=MasterWeb.db")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Curso>().ToTable("cursos");
            modelBuilder.Entity<Instructor>().ToTable("instructores");
            modelBuilder.Entity<CursoInstructor>().ToTable("cursos_instructores");
            modelBuilder.Entity<Precio>().ToTable("precios");
            modelBuilder.Entity<CursoPrecio>().ToTable("cursos_precios");
            modelBuilder.Entity<Calificacion>().ToTable("calificaciones");
            modelBuilder.Entity<Photo>().ToTable("photos");

            modelBuilder.Entity<Precio>()
                        .Property(b => b.PrecioActual)
                        .HasPrecision(10, 2);

            modelBuilder.Entity<Precio>()
                        .Property(b => b.PrecioPromocion)
                        .HasPrecision(10, 2);

            modelBuilder.Entity<Precio>()
                        .Property(b => b.Nombre)
                        .HasColumnType("VARCHAR")
                        .HasMaxLength(250);

            modelBuilder.Entity<Curso>()
                        .HasMany(c => c.Photos)
                        .WithOne(p => p.Curso)
                        .HasForeignKey(p => p.CursoId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Curso>()
                        .HasMany(c => c.Calificaciones)
                        .WithOne(c => c.Curso)
                        .HasForeignKey(c => c.CursoId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Curso>()
                        .HasMany(c => c.Precios)
                        .WithMany(p => p.Cursos)
                        .UsingEntity<CursoPrecio>(
                            j => j
                                  .HasOne(cp => cp.Precio)
                                  .WithMany(p => p.CursoPrecios)
                                  .HasForeignKey(cp => cp.PrecioId),
                            j => j
                                  .HasOne(cp => cp.Curso)
                                  .WithMany(c => c.CursoPrecios)
                                  .HasForeignKey(cp => cp.CursoId),
                            j =>
                            {
                                j.HasKey(t => new { t.PrecioId, t.CursoId });
                            }

                        );

            modelBuilder.Entity<Curso>()
                        .HasMany(c => c.Instructores)
                        .WithMany(i => i.Cursos)
                        .UsingEntity<CursoInstructor>(
                            j => j
                                  .HasOne(ci => ci.Instructor)
                                  .WithMany(i => i.CursosInstructors)
                                  .HasForeignKey(ci => ci.InstructorId),
                            j => j
                                  .HasOne(ci => ci.Curso)
                                  .WithMany(c => c.CursoInstructors)
                                  .HasForeignKey(ci => ci.CursoId),
                            j =>
                            {
                                j.HasKey(t => new { t.InstructorId, t.CursoId });
                            }
                        );

            modelBuilder.Entity<Curso>().HasData(DataMaster().Item1);
            modelBuilder.Entity<Precio>().HasData(DataMaster().Item2);
            modelBuilder.Entity<Instructor>().HasData(DataMaster().Item3);
        }

        public Tuple<Curso[], Precio[], Instructor[]> DataMaster()
        {
            List<Curso> cursos = new List<Curso>();
            Faker faker = new Faker();

            for (int i = 0; i < 10; i++)
            {
                Curso curso = new Curso()
                {
                    Id = Guid.NewGuid(),
                    Descripcion = faker.Commerce.ProductDescription(),
                    Titulo = faker.Commerce.ProductName(),
                    FechaDePublicacion = DateTime.UtcNow
                };
                cursos.Add(curso);
            }

            Precio precio = new Precio()
            {
                Id = Guid.NewGuid(),
                PrecioActual = 10.0m,
                PrecioPromocion = 8.0m,
                Nombre = "Precio Regular"
            };
            List<Precio> precios = new List<Precio>() { precio };

            Faker<Instructor> fakerInstructor = new Faker<Instructor>()
                .RuleFor(i => i.Id, _ => Guid.NewGuid())
                .RuleFor(i => i.Nombres, f => f.Name.FirstName())
                .RuleFor(i => i.Apellidos, f => f.Name.LastName())
                .RuleFor(i => i.Grado, f => f.Name.JobTitle());

            List<Instructor> instructores = fakerInstructor.Generate(10);

            return Tuple.Create(cursos.ToArray(), precios.ToArray(), instructores.ToArray());
        }
    }
}
