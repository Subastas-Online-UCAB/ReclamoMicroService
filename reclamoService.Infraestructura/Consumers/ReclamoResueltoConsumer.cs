using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using reclamoService.Aplicacion.Eventos;
using reclamoService.Dominio.Interfaces;

namespace reclamoService.Infraestructura.Consumers
{
    public class ReclamoResueltoConsumer : IConsumer<ReclamoResueltoEvent>
{
    private readonly IReclamoMongoRepository _mongo;

    public ReclamoResueltoConsumer(IReclamoMongoRepository mongo)
    {
        _mongo = mongo;
    }

    public async Task Consume(ConsumeContext<ReclamoResueltoEvent> context)
    {
        await _mongo.ActualizarEstadoAsync(context.Message.ReclamoId, context.Message.NuevoEstado);


    }
}
}

