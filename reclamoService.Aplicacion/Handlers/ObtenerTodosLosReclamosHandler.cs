using MediatR;
using reclamoService.Aplicacion.DTOs;
using reclamoService.Aplicacion.Queries;
using reclamoService.Dominio.Interfaces;

namespace reclamoService.Aplicacion.Handlers
{
    public class ObtenerTodosLosReclamosHandler : IRequestHandler<ObtenerTodosLosReclamosQuery, List<ReclamoDto>>
    {
        private readonly IReclamoMongoRepository _mongoRepo;

        public ObtenerTodosLosReclamosHandler(IReclamoMongoRepository mongoRepo)
        {
            _mongoRepo = mongoRepo;
        }

        public async Task<List<ReclamoDto>> Handle(ObtenerTodosLosReclamosQuery request, CancellationToken cancellationToken)
        {
            var reclamos = await _mongoRepo.ObtenerTodosAsync();

            return reclamos.Select(r => new ReclamoDto
            {
                Id = r.Id,
                UsuarioId = r.UsuarioId,
                SubastaId = r.SubastaId,
                Motivo = r.Motivo,
                Descripcion = r.Descripcion,
                FechaCreacion = r.FechaCreacion,
                Estado = r.Estado,
                Solucion = r.Solucion, // ✅ agregado
            }).ToList();
        }
    }
}