using MediatR;
using reclamoService.Aplicacion.Commands;
using reclamoService.Dominio.Interfaces;

namespace reclamoService.Aplicacion.Handlers;

public class resolverReclamoHandler : IRequestHandler<resolverReclamoCommand, bool>
{
    private readonly IReclamoRepository _repo;
    private readonly IReclamoEventPublisher _eventPublisher;

    public resolverReclamoHandler(IReclamoRepository repo, IReclamoEventPublisher eventPublisher)
    {
        _repo = repo;
        _eventPublisher = eventPublisher;
    }

    public async Task<bool> Handle(resolverReclamoCommand request, CancellationToken cancellationToken)
    {
        var actualizado = await _repo.ActualizarEstadoAsync(request.ReclamoId, "Resuelto");
        if (actualizado)
        {
            await _eventPublisher.PublicarReclamoResueltoAsync(request.ReclamoId);
        }

        return actualizado;
    }
}