using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reclamoService.Dominio.Entidades;
using reclamoService.Infraestructura.Persistencia;

namespace reclamoService.Test.SeedData
{
    public static class TestDbInitializer
    {
        public static void Seed(ReclamoDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Reclamos.Any()) return;

            context.Reclamos.AddRange(
                new reclamo
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    UsuarioId = "usuario_1",
                    SubastaId = "subasta_1",
                    Motivo = "Producto roto",
                    Descripcion = "El producto llegó dañado",
                    FechaCreacion = DateTime.UtcNow.AddDays(-3),
                    Estado = "Pendiente"
                },
                new reclamo
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    UsuarioId = "usuario_2",
                    SubastaId = "subasta_2",
                    Motivo = "No llegó",
                    Descripcion = "El producto nunca llegó",
                    FechaCreacion = DateTime.UtcNow.AddDays(-2),
                    Estado = "Pendiente"
                }
            );

            context.SaveChanges();
        }
    }
}
