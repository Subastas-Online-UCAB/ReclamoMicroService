 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Interfaces;
using reclamoService.Dominio.Excepciones;
using reclamoService.Infraestructura.Persistencia;

namespace reclamoService.Infraestructura.Repositorios
{
    // Infraestructura/Persistencia/PostgreSQL/ReclamoRepository.cs
    public class ReclamoRepository : IReclamoRepository
    {
        private readonly ReclamoDbContext _context;

        public ReclamoRepository(ReclamoDbContext context)
        {
            _context = context;
        }

        public async Task GuardarAsync(reclamo reclamo)
        {
            _context.Reclamos.Add(reclamo);
            await _context.SaveChangesAsync();
        }

        public async Task<reclamo?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Reclamos.FindAsync(id);
        }

        public async Task<IEnumerable<reclamo>> ObtenerPorUsuarioAsync(string usuarioId)
        {
            return await _context.Reclamos
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<reclamo>> ObtenerTodosAsync()
        {
            return await _context.Reclamos.ToListAsync();
        }

        public async Task<bool> ActualizarEstadoAsync(Guid id, string estado)
        {
            var reclamo = await _context.Reclamos.FindAsync(id);
            if (reclamo == null) return false;

            reclamo.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
