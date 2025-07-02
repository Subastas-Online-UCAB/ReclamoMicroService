using reclamoService.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace reclamoService.Infraestructura.Persistencia
{
    // Infraestructura/Persistencia/PostgreSQL/ReclamoDbContext.cs
    public class ReclamoDbContext : DbContext
    {
        public ReclamoDbContext(DbContextOptions<ReclamoDbContext> options) : base(options) { }
        public DbSet<reclamo> Reclamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<reclamo>(entity =>
            {
                entity.ToTable("Reclamos");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Motivo).IsRequired();
                entity.Property(r => r.Estado).HasDefaultValue("Pendiente");
                entity.Property(r => r.Solucion)
                    .HasColumnType("text") // o "varchar(500)" si quieres limitar
                    .IsRequired(false);    // explícitamente opcional
            });
        }
    }

}
