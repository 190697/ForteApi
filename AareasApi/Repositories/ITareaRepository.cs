using TareasApi.Controllers.ViewEntities;
using TareasApi.Entities;

namespace TareasApi.Repositories
{
    public interface ITareaRepository
    {
        Task<ViewTarea> GetTareaById(int idTarea);
        Task<List<Tarea>> GetTareas();
        Task<bool> AgregarTarea(Tarea tarea);
        Task<bool> ActualizarTarea(ViewTarea tarea);
        Task<bool> EliminarTarea(int idTarea);
    }
}
