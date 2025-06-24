using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using reclamoService.Aplicacion.Commands;
using reclamoService.Aplicacion.Handlers;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Interfaces;
using Xunit;

namespace ReclamoService.Tests.Handlers
{
   /* public class CrearReclamoHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ReturnsReclamoIdAndPublishesEvent()
        {
            // Arrange
            var repoMock = new Mock<IReclamoRepository>();
            var publisherMock = new Mock<IReclamoEventPublisher>();

            var handler = new CrearReclamoHandler(repoMock.Object, publisherMock.Object);

            var command = new createReclamoCommand("")
            {
                UsuarioId = Guid.NewGuid(),
                SubastaId = Guid.NewGuid(),
                Motivo = "Producto defectuoso",
                Descripcion = "El producto llegó roto"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);

            repoMock.Verify(r => r.GuardarAsync(It.Is<reclamo>(r =>
                r.UsuarioId == command.UsuarioId &&
                r.SubastaId == command.SubastaId &&
                r.Motivo == command.Motivo &&
                r.Descripcion == command.Descripcion &&
                r.Estado == "Pendiente"
            )), Times.Once);

            publisherMock.Verify(p => p.PublicarReclamoCreadoAsync(It.IsAny<reclamo>()), Times.Once);
        }
    }*/
}