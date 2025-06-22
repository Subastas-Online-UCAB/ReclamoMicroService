using MassTransit;
using reclamoService.Aplicacion.Eventos;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Infraestructura.EventPublisher
{
    public class ReclamoEventPublisher : IReclamoEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ReclamoEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublicarReclamoCreadoAsync(reclamo nuevoReclamo)
        {
            var evento = new ReclamoCreadoEvent
            {
                Id = nuevoReclamo.Id,
                UsuarioId = nuevoReclamo.UsuarioId,
                SubastaId = nuevoReclamo.SubastaId,
                Motivo = nuevoReclamo.Motivo,
                Descripcion = nuevoReclamo.Descripcion,
                FechaCreacion = nuevoReclamo.FechaCreacion
            };

            await _publishEndpoint.Publish(evento);
        }
    }

}
