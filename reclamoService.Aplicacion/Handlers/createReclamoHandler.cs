using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using reclamoService.Aplicacion.Commands;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Interfaces;

namespace reclamoService.Aplicacion.Handlers
{
    /// <summary>
    /// Handler que crea un nuevo reclamo en la base de datos y publica un evento para su procesamiento.
    /// </summary>
    public class CrearReclamoHandler : IRequestHandler<createReclamoCommand, Guid>
    {
        private readonly IReclamoRepository _repo;
        private readonly IReclamoEventPublisher _eventPublisher;

        public CrearReclamoHandler(IReclamoRepository repo, IReclamoEventPublisher eventPublisher)
        {
            _repo = repo;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> Handle(createReclamoCommand request, CancellationToken cancellationToken)
        {
            var reclamo = new reclamo
            {
                Id = Guid.NewGuid(),
                UsuarioId = request.UsuarioId,
                SubastaId = request.SubastaId,
                Motivo = request.Motivo,
                Descripcion = request.Descripcion,
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente"
            };

            await _repo.GuardarAsync(reclamo);
            await _eventPublisher.PublicarReclamoCreadoAsync(reclamo);

            return reclamo.Id;
        }
    }
}