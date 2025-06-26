using Microsoft.EntityFrameworkCore;
using TareasApi.Controllers.ViewEntities;
using TareasApi.Entities;
using TareasApi.Model;

namespace TareasApi.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly ApplicationDbContext _context;

        public TareaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tarea> GetTareaById(int idTarea)
        {
            return await _context.Tarea
                .Include(t => t.Estatus)
                .FirstOrDefaultAsync(t => t.Id == idTarea);
        }

        public async Task<List<Tarea>> GetTareas()
        {
            return await _context.Tarea
                .Include(t => t.Estatus)
                .ToListAsync();
        }

        public async Task<bool> AgregarTarea(Tarea tarea)
        {
            _context.Tarea.Add(tarea);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ActualizarTarea(ViewTarea tarea)
        {
            var tareaExistente = await _context.Tarea.FindAsync(tarea.Id);
            if (tareaExistente != null)
            {
                tareaExistente.Titulo = tarea.Titulo;
                tareaExistente.Descripcion = tarea.Descripcion;
                tareaExistente.Id_Estatus = tarea.Id_Estatus;
                await _context.SaveChangesAsync();

                return true;
            }

            return false;

        }

        public async Task<bool> EliminarTarea(int idTarea)
        {
            var tarea = await _context.Tarea.FindAsync(idTarea);
            if (tarea != null)
            {
                _context.Tarea.Remove(tarea);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}