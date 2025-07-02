using MediatR;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Excepciones;
using reclamoService.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using reclamoService.Aplicacion.Commands;

public class AgregarSolucionReclamoHandler : IRequestHandler<AgregarSolucionReclamoCommand, Unit>
{
    private readonly IReclamoRepository _reclamoRepository;
    private readonly IReclamoEventPublisher _eventPublisher;

    public AgregarSolucionReclamoHandler(
        IReclamoRepository reclamoRepository,
        IReclamoEventPublisher eventPublisher)
    {
        _reclamoRepository = reclamoRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Unit> Handle(AgregarSolucionReclamoCommand request, CancellationToken cancellationToken)
    {
        var reclamo = await _reclamoRepository.ObtenerPorIdAsync(request.ReclamoId);
        if (reclamo == null)
            throw new reclamoNoEncontradoException(request.ReclamoId);

        reclamo.AgregarSolucion(request.Solucion);

        await _reclamoRepository.ActualizarAsync(reclamo);

        await _eventPublisher.PublicarSolucionAgregadaAsync(
            reclamo.Id,
            reclamo.Solucion!,
            DateTime.UtcNow
        );

        return Unit.Value;
    }
}