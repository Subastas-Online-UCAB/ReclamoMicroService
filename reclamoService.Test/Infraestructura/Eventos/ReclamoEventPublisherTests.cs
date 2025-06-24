using System;
using System.Threading.Tasks;
using Moq;
using MassTransit;
using reclamoService.Dominio.Entidades;
using reclamoService.Aplicacion.Eventos;
using reclamoService.Infraestructura.EventPublisher;
using Xunit;

namespace reclamoService.Test.Infraestructura.Eventos
{
    public class ReclamoEventPublisherTests
    {
        [Fact]
        public async Task PublicarReclamoCreadoAsync_Should_Publish_ReclamoCreadoEvent()
        {
            // Arrange
            var mockPublisher = new Mock<IPublishEndpoint>();
            var publisher = new ReclamoEventPublisher(mockPublisher.Object);

            var reclamo = new reclamo
            {
                Id = Guid.NewGuid(),
                UsuarioId = "user123",
                SubastaId = "subasta456",
                Motivo = "Producto defectuoso",
                Descripcion = "El artículo llegó dañado.",
                FechaCreacion = DateTime.UtcNow
            };

            // Act
            await publisher.PublicarReclamoCreadoAsync(reclamo);

            // Assert
            mockPublisher.Verify(x => x.Publish(
                It.Is<ReclamoCreadoEvent>(e =>
                    e.Id == reclamo.Id &&
                    e.UsuarioId == reclamo.UsuarioId &&
                    e.SubastaId == reclamo.SubastaId &&
                    e.Motivo == reclamo.Motivo &&
                    e.Descripcion == reclamo.Descripcion &&
                    e.FechaCreacion == reclamo.FechaCreacion
                ),
                default), Times.Once);
        }

        [Fact]
        public async Task PublicarReclamoResueltoAsync_Should_Publish_ReclamoResueltoEvent()
        {
            // Arrange
            var mockPublisher = new Mock<IPublishEndpoint>();
            var publisher = new ReclamoEventPublisher(mockPublisher.Object);

            var reclamoId = Guid.NewGuid();

            // Act
            await publisher.PublicarReclamoResueltoAsync(reclamoId);

            // Assert
            mockPublisher.Verify(x => x.Publish(
                It.Is<ReclamoResueltoEvent>(e =>
                    e.ReclamoId == reclamoId &&
                    e.NuevoEstado == "Resuelto"
                ),
                default), Times.Once);
        }
    }
}
