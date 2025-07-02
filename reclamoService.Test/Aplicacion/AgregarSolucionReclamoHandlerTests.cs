using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using reclamoService.Aplicacion.Handlers;
using reclamoService.Dominio.Entidades;
using reclamoService.Dominio.Excepciones;
using reclamoService.Dominio.Interfaces;
using MediatR;
using reclamoService.Aplicacion.Commands;

public class AgregarSolucionReclamoHandlerTests
{
    private readonly Mock<IReclamoRepository> _reclamoRepoMock;
    private readonly Mock<IReclamoEventPublisher> _publisherMock;
    private readonly AgregarSolucionReclamoHandler _handler;

    public AgregarSolucionReclamoHandlerTests()
    {
        _reclamoRepoMock = new Mock<IReclamoRepository>();
        _publisherMock = new Mock<IReclamoEventPublisher>();
        _handler = new AgregarSolucionReclamoHandler(_reclamoRepoMock.Object, _publisherMock.Object);
    }

    [Fact]
    public async Task Handle_ReclamoExiste_ActualizaYPublicaEvento()
    {
        // Arrange
        var reclamoId = Guid.NewGuid();
        var solucion = "Producto reemplazado.";

        var nuevoReclamo = new reclamo
        {
            Id = reclamoId,
            UsuarioId = "abc",
            SubastaId = "abc",
            Motivo = "Motivo",
            Descripcion = "Descripción",
            Estado = "Pendiente",
            FechaCreacion = DateTime.UtcNow
        };

        _reclamoRepoMock
            .Setup(r => r.ObtenerPorIdAsync(reclamoId))
            .ReturnsAsync(nuevoReclamo);

        var command = new AgregarSolucionReclamoCommand(reclamoId, solucion);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        Assert.Equal("Resuelto", nuevoReclamo.Estado);
        Assert.Equal(solucion, nuevoReclamo.Solucion);

        _reclamoRepoMock.Verify(r => r.ActualizarAsync(nuevoReclamo), Times.Once);
        _publisherMock.Verify(p => p.PublicarSolucionAgregadaAsync(reclamoId, solucion, It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReclamoNoExiste_LanzaExcepcion()
    {
        // Arrange
        var reclamoId = Guid.NewGuid();

        _reclamoRepoMock
            .Setup(r => r.ObtenerPorIdAsync(reclamoId))
            .ReturnsAsync((reclamo)null!);

        var command = new AgregarSolucionReclamoCommand(reclamoId, "solucion");

        // Act & Assert
        await Assert.ThrowsAsync<reclamoNoEncontradoException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _reclamoRepoMock.Verify(r => r.ActualizarAsync(It.IsAny<reclamo>()), Times.Never);
        _publisherMock.Verify(p => p.PublicarSolucionAgregadaAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
    }
}
