using MongoDB.Driver;
using reclamoService.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reclamoService.Dominio.Interfaces;

namespace reclamoService.Infraestructura.Mongo
{
    public class ReclamoMongoRepository : IReclamoMongoRepository
    {
        private readonly IMongoCollection<reclamo> _reclamos;

        public ReclamoMongoRepository(IMongoDatabase database)
        {
            _reclamos = database.GetCollection<reclamo>("Reclamos");
        }

        public async Task GuardarAsync(reclamo reclamo)
        {
            await _reclamos.InsertOneAsync(reclamo);
        }

        public async Task<List<reclamo>> ObtenerTodosAsync()
        {
            return await _reclamos.Find(_ => true).ToListAsync();
        }

        public async Task ActualizarEstadoAsync(Guid reclamoId, string nuevoEstado)
        {
            var filter = Builders<reclamo>.Filter.Eq(x => x.Id, reclamoId);
            var update = Builders<reclamo>.Update.Set(x => x.Estado, nuevoEstado);

            await _reclamos.UpdateOneAsync(filter, update);
        }

    }

}
