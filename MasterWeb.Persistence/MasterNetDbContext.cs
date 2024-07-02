using MasterWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace MasterWeb.Persistence
{
    public class MasterNetDbContext : DbContext
    {
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
        }
    }
}
