using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Aplicacion.Eventos
{
    /// <summary>
    /// Evento que representa la creación de un reclamo.
    /// </summary>
    public class ReclamoCreadoEvent
    {
        public Guid Id { get; set; }
        public string UsuarioId { get; set; } = null!;
        public string SubastaId { get; set; } = null!;
        public string Motivo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
    }
}
