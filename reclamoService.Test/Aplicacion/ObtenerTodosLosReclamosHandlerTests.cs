using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using reclamoService.Aplicacion.Handlers;
using reclamoService.Aplicacion.Queries;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Interfaces;
using reclamoService.Aplicacion.DTOs;

namespace reclamoService.Tests.Aplicacion.Handlers
{
    public class ObtenerTodosLosReclamosHandlerTests
    {
        [Fact]
        public async Task Handle_CuandoHayReclamos_RetornaListaDeDtos()
        {
            // Arrange
            var mockRepo = new Mock<IReclamoMongoRepository>();

            var reclamos = new List<reclamo>
            {
                new reclamo
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = "user1",
                    SubastaId = "subasta1",
                    Motivo = "Motivo1",
                    Descripcion = "Descripcion1",
                    FechaCreacion = DateTime.UtcNow,
                    Estado = "Pendiente",
                    Solucion = "Aún no resuelto"
                },
                new reclamo
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = "user2",
                    SubastaId = "subasta2",
                    Motivo = "Motivo2",
                    Descripcion = "Descripcion2",
                    FechaCreacion = DateTime.UtcNow,
                    Estado = "Resuelto",
                    Solucion = "Se envió reemplazo"
                }
            };

            mockRepo.Setup(r => r.ObtenerTodosAsync())
                    .ReturnsAsync(reclamos);

            var handler = new ObtenerTodosLosReclamosHandler(mockRepo.Object);
            var query = new ObtenerTodosLosReclamosQuery();

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);

            Assert.Equal("user1", resultado[0].UsuarioId);
            Assert.Equal("Motivo1", resultado[0].Motivo);
            Assert.Equal("Aún no resuelto", resultado[0].Solucion);

            Assert.Equal("user2", resultado[1].UsuarioId);
            Assert.Equal("Motivo2", resultado[1].Motivo);
            Assert.Equal("Se envió reemplazo", resultado[1].Solucion);
        }
    }
}
