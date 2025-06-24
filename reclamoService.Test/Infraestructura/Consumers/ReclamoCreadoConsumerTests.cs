using System;
using System.Threading.Tasks;
using Moq;
using MassTransit;
using reclamoService.Aplicacion.Eventos;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Interfaces;
using reclamoService.Infraestructura.Consumers;
using Xunit;

namespace ReclamoService.Tests.Infraestructura.Consumers
{
    public class ReclamoCreadoConsumerTests
    {
        [Fact]
        public async Task Consume_Should_SaveReclamoInMongo()
        {
            // Arrange
            var mockRepo = new Mock<IReclamoMongoRepository>();
            var consumer = new ReclamoCreadoConsumer(mockRepo.Object);

            var evento = new ReclamoCreadoEvent
            {
                Id = Guid.NewGuid(),
                UsuarioId = "user001",
                SubastaId = "subasta001",
                Motivo = "Motivo de prueba",
                Descripcion = "Descripción del reclamo",
                FechaCreacion = DateTime.UtcNow
            };

            var mockContext = new Mock<ConsumeContext<ReclamoCreadoEvent>>();
            mockContext.Setup(c => c.Message).Returns(evento);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockRepo.Verify(r => r.GuardarAsync(It.Is<reclamo>(x =>
                x.Id == evento.Id &&
                x.UsuarioId == evento.UsuarioId &&
                x.SubastaId == evento.SubastaId &&
                x.Motivo == evento.Motivo &&
                x.Descripcion == evento.Descripcion &&
                x.FechaCreacion == evento.FechaCreacion &&
                x.Estado == "Pendiente"
            )), Times.Once);
        }
    }
}