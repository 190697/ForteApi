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

        public async Task<ViewTarea> GetTareaById(int idTarea)
        {
            return await (from m in _context.Tarea
                          where
                              m.Id.Equals(idTarea)
                          select new ViewTarea
                          {
                            Id = m.Id,
                            Id_Estatus = m.Id_Estatus,
                            Titulo = m.Titulo,
                            Descripcion = m.Descripcion,
                            Fecha_Registro = m.Fecha_Registro,
                          }).FirstOrDefaultAsync();
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