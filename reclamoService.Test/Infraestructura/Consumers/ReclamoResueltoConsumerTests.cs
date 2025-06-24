using System;
using System.Threading.Tasks;
using MassTransit;
using Moq;
using reclamoService.Aplicacion.Eventos;
using reclamoService.Dominio.Interfaces;
using reclamoService.Infraestructura.Consumers;
using Xunit;

namespace ReclamoService.Tests.Infraestructura.Consumers
{
    public class ReclamoResueltoConsumerTests
    {
        [Fact]
        public async Task Consume_Should_UpdateReclamoEstadoInMongo()
        {
            // Arrange
            var mockRepo = new Mock<IReclamoMongoRepository>();
            var consumer = new ReclamoResueltoConsumer(mockRepo.Object);

            var evento = new ReclamoResueltoEvent
            {
                ReclamoId = Guid.NewGuid(),
                NuevoEstado = "Resuelto"
            };

            var mockContext = new Mock<ConsumeContext<ReclamoResueltoEvent>>();
            mockContext.Setup(c => c.Message).Returns(evento);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockRepo.Verify(r => r.ActualizarEstadoAsync(evento.ReclamoId, evento.NuevoEstado), Times.Once);
        }
    }
}