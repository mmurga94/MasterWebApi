namespace MasterWeb.Domain
{
    public class Curso: BaseEntity
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaDePublicacion { get; set; }
        public ICollection<Calificacion>? Calificaciones { get; set; }
        public ICollection<Precio>? Precios { get; set; }
        public ICollection<CursoPrecio>? CursoPrecios { get; set; }
        public ICollection<Instructor>? Instructores { get; set; }
        public ICollection<CursoInstructor>? CursoInstructors { get; set; }
        public ICollection<Photo>? Photos { get; set; }
    }
}
