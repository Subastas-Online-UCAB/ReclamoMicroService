using MediatR;
using System;

namespace reclamoService.Aplicacion.Commands
{
    /// <summary>
    /// Comando para crear un nuevo reclamo relacionado con una subasta.
    /// </summary>
    public class createReclamoCommand : IRequest<Guid>
    {
        /// <summary>
        /// ID del usuario que realiza el reclamo.
        /// </summary>
        public string UsuarioId { get; set; }

        /// <summary>
        /// ID de la subasta asociada al reclamo.
        /// </summary>
        public string SubastaId { get; set; }

        /// <summary>
        /// Motivo principal del reclamo.
        /// </summary>
        public string Motivo { get; set; } = null!;

        /// <summary>
        /// Descripción detallada del reclamo.
        /// </summary>
        public string Descripcion { get; set; } = null!;

        /// <summary>
        /// Inicializa el comando con los datos del reclamo.
        /// </summary>
        /// <param name="usuarioId">ID del usuario.</param>
        /// <param name="subastaId">ID de la subasta.</param>
        /// <param name="motivo">Motivo del reclamo.</param>
        /// <param name="descripcion">Descripción del problema.</param>
        public createReclamoCommand(string usuarioId, string subastaId, string motivo, string descripcion)
        {
            this.UsuarioId = usuarioId;
            this.SubastaId = subastaId;
            this.Motivo = motivo;
            this.Descripcion = descripcion;
        }
    }
}