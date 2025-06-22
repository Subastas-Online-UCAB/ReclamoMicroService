using reclamoService.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Dominio.Interfaces
{
    public interface IReclamoEventPublisher
    {
        Task PublicarReclamoCreadoAsync(reclamo nuevoReclamo);
    }
}
