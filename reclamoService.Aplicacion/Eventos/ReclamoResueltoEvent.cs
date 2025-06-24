using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Aplicacion.Eventos
{
    public class ReclamoResueltoEvent
    {
        public Guid ReclamoId { get; set; }
        public string NuevoEstado { get; set; } = "Resuelto";
    }

}
