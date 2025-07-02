using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Aplicacion.Eventos
{
    public class SolucionReclamoAgregadaEvent
    {
        public Guid ReclamoId { get; set; }
        public string Solucion { get; set; }
    }

}
