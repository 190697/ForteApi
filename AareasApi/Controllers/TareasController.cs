using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TareasApi.Controllers.ViewEntities;
using TareasApi.Entities;
using TareasApi.Model;
using TareasApi.Repositories;

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
        private readonly ITareaRepository _tareaRepository;

        /// <summary>
        /// Constructor que recibe el contexto del repositorio.
        /// </summary>
        /// <param name="tareaRepository">Instancia del repositorio.</param>
        public TareasController(ITareaRepository tareaRepository)
        {
            this._tareaRepository = tareaRepository;
        }

        private readonly ILogger<TareasController> _logger;

        /// <summary>
        /// Obtiene la lista de todas las tareas.
        /// </summary>
        /// <returns>Lista de tareas.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _tareaRepository.GetTareas();

                return Ok(result.Select(m => new
                {
                    m.Id,
                    m.Id_Estatus,
                    m.Titulo,
                    m.Descripcion,
                    m.Fecha_Registro,
                    Estatus = m.Estatus.Nombre
                }));
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
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Tarea search = await _tareaRepository.GetTareaById(id);

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
        public async Task<IActionResult> Post([FromBody] ViewTarea oTarea)
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

                if(await _tareaRepository.AgregarTarea(newTarea))
                    return Ok();

                return StatusCode(StatusCodes.Status500InternalServerError);

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
        public async Task<IActionResult> Put([FromBody] ViewTarea oTarea)
        {
            try
            {
                if (string.IsNullOrEmpty(oTarea.Titulo))
                    return BadRequest("El titulo es requerido.");

                if(await _tareaRepository.ActualizarTarea(oTarea))
                {
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if(await _tareaRepository.EliminarTarea(id)) {
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