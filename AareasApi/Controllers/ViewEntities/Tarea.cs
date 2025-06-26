using TareasApi.Entities;

namespace TareasApi.Controllers.ViewEntities
{
    public class ViewTarea
    {
        public int Id { get; set; }
        public int Id_Estatus { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha_Registro { get; set; } = DateTime.Now;
    }
}

