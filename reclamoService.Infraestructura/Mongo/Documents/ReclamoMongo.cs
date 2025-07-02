using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Infraestructura.Mongo.Documents
{
    public class ReclamoMongo
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string? Solucion { get; set; }
    }

}
