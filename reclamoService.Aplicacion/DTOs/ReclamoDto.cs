using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Aplicacion.DTOs
{
    public class ReclamoDto
    {
        public Guid Id { get; set; }
        public string UsuarioId { get; set; }
        public string SubastaId { get; set; }
        public string Motivo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }


        public string? Solucion { get; set; } // ✅ nueva
    }
}
