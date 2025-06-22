using reclamoService.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Dominio.Interfaces
{
    public interface IReclamoMongoRepository
    {
        Task<List<reclamo>> ObtenerTodosAsync();
        Task GuardarAsync(reclamo reclamo);
    }
}
