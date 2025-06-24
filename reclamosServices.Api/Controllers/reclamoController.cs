using MediatR;
using Microsoft.AspNetCore.Mvc;
using reclamoService.Aplicacion.Commands;
using reclamoService.Aplicacion.DTOs;
using reclamoService.Aplicacion.Queries;
using reclamoService.Dominio.Excepciones;

namespace reclamosServices.Api.Controllers
{
    /// <summary>
    /// Permite a los usuarios enviar reclamos relacionados con subastas.
    /// </summary>
    [ApiController]
    [Route("api/reclamos")]
    public class ReclamoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReclamoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea un nuevo reclamo enviado por un usuario.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite registrar un reclamo indicando el motivo, la subasta y una descripción.
        /// </remarks>
        /// <param name="cmd">Objeto con los datos del reclamo (usuario, subasta, motivo y descripción).</param>
        /// <returns>Devuelve un código 201 con el ID del reclamo creado.</returns>
        /// <response code="201">Reclamo creado exitosamente.</response>
        /// <response code="400">Datos inválidos o incompletos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Crear([FromBody] createReclamoCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Crear), new { id }, new { id });
        }


        /// <summary>
        /// Obtiene todos los reclamos registrados en el sistema.
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve una lista de reclamos almacenados en MongoDB, incluyendo información del usuario, subasta, motivo, descripción, fecha de creación y estado.
        /// </remarks>
        /// <returns>Lista de objetos <see cref="ReclamoDto"/>.</returns>
        /// <response code="200">Lista de reclamos obtenida correctamente.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ReclamoDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ReclamoDto>>> Get()
        {
            var result = await _mediator.Send(new ObtenerTodosLosReclamosQuery());
            return Ok(result);
        }


        /// <summary>
        /// Marca un reclamo como resuelto.
        /// </summary>
        /// <param name="id">ID del reclamo a resolver.</param>
        /// <returns>Devuelve 200 si fue exitoso, 404 si no se encontró el reclamo.</returns>
        /// <response code="200">Reclamo resuelto correctamente.</response>
        /// <response code="404">No se encontró el reclamo con el ID proporcionado.</response>
        [HttpPut("resolver/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResolverReclamo(Guid id)
        {
            var resultado = await _mediator.Send(new resolverReclamoCommand(id));

            if (!resultado)
                throw new reclamoNoEncontradoException(id); // Excepción personalizada

            return Ok(new { mensaje = "Reclamo resuelto correctamente." });
        }
    }
}