namespace MasterWeb.Domain
{
    public class Instructor: BaseEntity
    {
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Grado { get; set; }
        public ICollection<Curso>? Cursos { get; set; }
        public ICollection<CursoInstructor>? CursosInstructors { get; set; }
    }
}
