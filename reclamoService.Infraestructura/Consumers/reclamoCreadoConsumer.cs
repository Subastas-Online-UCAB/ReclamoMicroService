// ------------------------------
// ReclamoCreadoConsumer.cs
// ------------------------------
using MassTransit;
using reclamoService.Aplicacion.Eventos;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Interfaces;
using reclamoService.Infraestructura.Mongo;

namespace reclamoService.Infraestructura.Consumers
{
    /// <summary>
    /// Consumer que maneja eventos de reclamos creados.
    /// Guarda el reclamo recibido en la base de datos MongoDB.
    /// </summary>
    public class ReclamoCreadoConsumer : IConsumer<ReclamoCreadoEvent>
    {
        private readonly IReclamoMongoRepository _repo;

        public ReclamoCreadoConsumer(IReclamoMongoRepository repo)
        {
            _repo = repo;
        }

        public async Task Consume(ConsumeContext<ReclamoCreadoEvent> context)
        {
            var mensaje = context.Message;

            var nuevoReclamo = new reclamo
            {
                Id = mensaje.Id,
                UsuarioId = mensaje.UsuarioId,
                SubastaId = mensaje.SubastaId,
                Motivo = mensaje.Motivo,
                Descripcion = mensaje.Descripcion,
                FechaCreacion = mensaje.FechaCreacion,
                Estado = "Pendiente"
            };

            await _repo.GuardarAsync(nuevoReclamo);
        }
    }
}