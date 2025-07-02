using MassTransit;
using MongoDB.Driver;
using reclamoService.Aplicacion.Eventos;
using reclamoService.Infraestructura.Mongo.Documents;
public class SolucionReclamoAgregadaConsumer : IConsumer<SolucionReclamoAgregadaEvent>
{
    private readonly IMongoCollection<ReclamoMongo> _collection;

    public SolucionReclamoAgregadaConsumer(IMongoDatabase database)
    {
        _collection = database.GetCollection<ReclamoMongo>("Reclamos");
    }

    public async Task Consume(ConsumeContext<SolucionReclamoAgregadaEvent> context)
    {
        var message = context.Message;

        var filter = Builders<ReclamoMongo>.Filter.Eq(r => r.Id, message.ReclamoId);

        var update = Builders<ReclamoMongo>.Update
            .Set(r => r.Solucion, message.Solucion)
            .Set(r => r.Estado, "Resuelto");

        await _collection.UpdateOneAsync(filter, update);
    }
}