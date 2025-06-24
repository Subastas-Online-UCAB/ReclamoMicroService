using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace reclamoService.Dominio.Entidades
{

    public class reclamo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string UsuarioId { get; set; } 


        public string SubastaId { get; set; }
        public string Motivo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = "Pendiente"; // Puede ser EnRevision, Aprobado, Rechazado


        
    }

}
