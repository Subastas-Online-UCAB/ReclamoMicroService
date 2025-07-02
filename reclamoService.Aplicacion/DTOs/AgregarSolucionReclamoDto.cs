using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Aplicacion.DTOs
{
    public class AgregarSolucionReclamoDto
    {
        public Guid ReclamoId { get; set; }
        public string Solucion { get; set; }
    }

}
