using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reclamoService.Dominio.Entidades;
using reclamoService.Infraestructura.Persistencia;
using reclamoService.Infraestructura.Repositorios;
using Xunit;

namespace reclamoService.Tests.Infraestructura
{
    public class ReclamoRepositoryTests
    {
        private ReclamoDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ReclamoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ReclamoDbContext(options);
        }

        [Fact]
        public async Task GuardarAsync_Should_Save_Reclamo()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new ReclamoRepository(context);

            var reclamo = new reclamo
            {
                Id = Guid.NewGuid(),
                UsuarioId = "user-123",
                SubastaId = "Pruebas",
                Motivo = "Producto defectuoso",
                Descripcion = "El producto llegó roto",
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente"
            };

            // Act
            await repo.GuardarAsync(reclamo);

            // Assert
            var saved = await context.Reclamos.FindAsync(reclamo.Id);
            Assert.NotNull(saved);
            Assert.Equal("Producto defectuoso", saved.Motivo);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_Should_Return_Reclamo_When_Exists()
        {
            var context = GetInMemoryDbContext();
            var repo = new ReclamoRepository(context);
            var id = Guid.NewGuid();

            context.Reclamos.Add(new reclamo
            {
                Id = id,
                UsuarioId = "test-user",
                SubastaId = "Pruebas",
                Motivo = "Motivo de prueba",
                Descripcion = "Descripción",
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente"
            });
            await context.SaveChangesAsync();

            var result = await repo.ObtenerPorIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("test-user", result.UsuarioId);
        }

        [Fact]
        public async Task ActualizarEstadoAsync_Should_Update_Estado()
        {
            var context = GetInMemoryDbContext();
            var repo = new ReclamoRepository(context);
            var id = Guid.NewGuid();

            context.Reclamos.Add(new reclamo
            {
                Id = id,
                UsuarioId = "test-user",
                SubastaId = "Pruebas",
                Motivo = "Motivo",
                Descripcion = "Descripción",
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente"
            });
            await context.SaveChangesAsync();

            var updated = await repo.ActualizarEstadoAsync(id, "Resuelto");

            var result = await context.Reclamos.FindAsync(id);

            Assert.True(updated);
            Assert.Equal("Resuelto", result.Estado);
        }

        [Fact]
        public async Task ObtenerPorUsuarioAsync_Should_Return_Only_User_Reclamos()
        {
            var context = GetInMemoryDbContext();
            var repo = new ReclamoRepository(context);

            context.Reclamos.AddRange(
                new reclamo { Id = Guid.NewGuid(), UsuarioId = "user1", SubastaId = "subasta1", Motivo = "A", Descripcion = "X", FechaCreacion = DateTime.UtcNow, Estado = "Pendiente" },
                new reclamo { Id = Guid.NewGuid(), UsuarioId = "user2", SubastaId = "subasta2", Motivo = "B", Descripcion = "Y", FechaCreacion = DateTime.UtcNow, Estado = "Pendiente" }
            );
            await context.SaveChangesAsync();

            var result = await repo.ObtenerPorUsuarioAsync("user1");

            Assert.Single(result);
            Assert.Equal("user1", result.First().UsuarioId);
        }

        [Fact]
        public async Task ObtenerTodosAsync_Should_Return_All_Reclamos()
        {
            var context = GetInMemoryDbContext();
            var repo = new ReclamoRepository(context);

            context.Reclamos.AddRange(
                new reclamo { Id = Guid.NewGuid(), UsuarioId = "user1", SubastaId = "subasta1", Motivo = "A", Descripcion = "X", FechaCreacion = DateTime.UtcNow, Estado = "Pendiente" },
                new reclamo { Id = Guid.NewGuid(), UsuarioId = "user2", SubastaId = "subasta2", Motivo = "B", Descripcion = "Y", FechaCreacion = DateTime.UtcNow, Estado = "Pendiente" }
            );
            await context.SaveChangesAsync();

            var result = await repo.ObtenerTodosAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ActualizarEstadoAsync_Should_Update_Estado_Correctly()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new ReclamoRepository(context);
            var reclamoId = Guid.NewGuid();

            var reclamo = new reclamo
            {
                Id = reclamoId,
                UsuarioId = "usuario-prueba",
                SubastaId = "Pruebas",
                Motivo = "Retraso en entrega",
                Descripcion = "La subasta terminó pero no recibí mi producto",
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente"
            };

            context.Reclamos.Add(reclamo);
            await context.SaveChangesAsync();

            // Act
            var actualizado = await repo.ActualizarEstadoAsync(reclamoId, "Resuelto");

            // Assert
            Assert.True(actualizado);

            var reclamoActualizado = await context.Reclamos.FindAsync(reclamoId);
            Assert.NotNull(reclamoActualizado);
            Assert.Equal("Resuelto", reclamoActualizado.Estado);
            Assert.Equal("Retraso en entrega", reclamoActualizado.Motivo); // campo adicional validado
        }

        [Fact]
        public async Task ActualizarEstadoAsync_Should_ReturnFalse_WhenReclamoNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repo = new ReclamoRepository(context);
            var idInexistente = Guid.NewGuid(); // No lo agregamos al contexto

            // Act
            var result = await repo.ActualizarEstadoAsync(idInexistente, "Resuelto");

            // Assert
            Assert.False(result);
        }

    }
}
