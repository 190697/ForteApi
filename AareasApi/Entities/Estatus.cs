namespace TareasApi.Entities
{
    public class Estatus
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<Tarea> Tareas { get; set; }
    }
}
