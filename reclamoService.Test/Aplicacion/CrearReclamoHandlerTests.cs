using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using reclamoService.Aplicacion.Commands;
using reclamoService.Aplicacion.Handlers;
using reclamoService.Dominio.Interfaces;
using reclamoService.Dominio.Entidades;

namespace reclamoService.Tests.Aplicacion.Handlers
{
    public class CrearReclamoHandlerTests
    {
        [Fact]
        public async Task Handle_DeberiaCrearReclamoYPublicarEvento()
        {
            // Arrange
            var mockRepo = new Mock<IReclamoRepository>();
            var mockPublisher = new Mock<IReclamoEventPublisher>();

            var handler = new CrearReclamoHandler(mockRepo.Object, mockPublisher.Object);

            var comando = new createReclamoCommand(
                "user123",
                "subasta123",
                "Producto defectuoso",
                "El producto llegó roto"
            );

            // Act
            var resultado = await handler.Handle(comando, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GuardarAsync(It.Is<reclamo>(r =>
                r.UsuarioId == comando.UsuarioId &&
                r.SubastaId == comando.SubastaId &&
                r.Motivo == comando.Motivo &&
                r.Descripcion == comando.Descripcion &&
                r.Estado == "Pendiente"
            )), Times.Once);

            mockPublisher.Verify(p => p.PublicarReclamoCreadoAsync(It.IsAny<reclamo>()), Times.Once);

            Assert.NotEqual(Guid.Empty, resultado);
        }
    }
}