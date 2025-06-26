using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TareasApi.Controllers.ViewEntities;
using TareasApi.Entities;
using TareasApi.Model;

namespace TareasApi.Controllers
{
    /// <summary>
    /// Controlador API para gestionar tareas.
    /// Proporciona endpoints para operaciones CRUD sobre la entidad Tarea.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public TareasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        private readonly ILogger<TareasController> _logger;

        /// <summary>
        /// Obtiene la lista de todas las tareas.
        /// </summary>
        /// <returns>Lista de tareas.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = (from m in context.Tarea
                              select new
                              {
                                  m.Id,
                                  m.Id_Estatus,
                                  m.Titulo,
                                  m.Descripcion,
                                  m.Fecha_Registro,
                                  Estatus = m.Estatus.Nombre
                              }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Obtiene una tarea por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la tarea.</param>
        /// <returns>La tarea encontrada o null si no existe.</returns>
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                Tarea search = (from m in context.Tarea
                                where
                                   m.Id.Equals(id)
                                select m).FirstOrDefault();

                if (search == null)
                    return NotFound("No se encontro tarea.");

                return Ok(search);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Crea una nueva tarea.
        /// </summary>
        /// <param name="oTarea">Objeto tarea a crear.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] ViewTarea oTarea)
        {
            try
            {
                if (string.IsNullOrEmpty(oTarea.Titulo))
                    return BadRequest("El titulo es requerido.");

                Tarea newTarea = new Tarea()
                {
                    Id_Estatus = oTarea.Id_Estatus,
                    Titulo = oTarea.Titulo,
                    Descripcion = oTarea.Descripcion,
                    Fecha_Registro = DateTime.Now,
                };

                context.Tarea.Add(newTarea);
                context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una tarea existente.
        /// </summary>
        /// <param name="oTarea">Objeto tarea con los datos actualizados.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut]
        public IActionResult Put([FromBody] ViewTarea oTarea)
        {
            try
            {
                if (string.IsNullOrEmpty(oTarea.Titulo))
                    return BadRequest("El titulo es requerido.");

                Tarea search = (from m in context.Tarea
                                where
                                    m.Id.Equals(oTarea.Id)
                                select m).FirstOrDefault();

                if (search != null)
                {
                    search.Titulo = oTarea.Titulo ?? search.Titulo;
                    search.Descripcion = oTarea.Descripcion ?? search.Descripcion;
                    search.Id_Estatus = oTarea.Id_Estatus > 0 ? oTarea.Id_Estatus : search.Id_Estatus;

                    context.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound("No se encontro tarea.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Elimina una tarea por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la tarea a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                Tarea search = (from m in context.Tarea
                                where
                                    m.Id.Equals(id)
                                select m).FirstOrDefault();

                if (search != null)
                {
                    context.Tarea.Remove(search);
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound("No se encontro tarea.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}