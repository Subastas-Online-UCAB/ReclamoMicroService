using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using reclamoService.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Infraestructura.Mongo
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDb:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDb:Database").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<reclamo> Reclamos => _database.GetCollection<reclamo>("Reclamos");
    }
}
