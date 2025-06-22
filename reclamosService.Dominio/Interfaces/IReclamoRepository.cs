using reclamoService.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Dominio.Interfaces
{
    public interface IReclamoRepository
    {
        Task GuardarAsync(reclamo reclamo);
        Task<reclamo?> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<reclamo>> ObtenerPorUsuarioAsync(string usuarioId);
        Task<IEnumerable<reclamo>> ObtenerTodosAsync();
        Task ActualizarEstadoAsync(Guid id, string nuevoEstado);
    }
}
