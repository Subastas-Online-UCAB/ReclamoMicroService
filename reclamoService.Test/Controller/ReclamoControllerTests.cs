using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using reclamoService.Aplicacion.Commands;
using reclamoService.Aplicacion.DTOs;
using reclamoService.Aplicacion.Queries;
using reclamoService.Dominio.Excepciones;
using reclamosServices.Api.Controllers;
using Xunit;

namespace ReclamoService.Tests.Controllers
{
    public class ReclamoControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ReclamoController _controller;

        public ReclamoControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ReclamoController(_mediatorMock.Object);
        }

       

        [Fact]
        public async Task Crear_ReturnsCreatedResultWithId()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new ReclamoController(mockMediator.Object);

            var idEsperado = Guid.NewGuid();

            mockMediator
                .Setup(m => m.Send(It.IsAny<createReclamoCommand>(), default))
                .ReturnsAsync(idEsperado);

            var command = new createReclamoCommand("Pueba", "Pueba", "Motivo", "Desc");

            // Act
            var result = await controller.Crear(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            // 👇 Convierte a JSON y luego a diccionario para extraer el id
            var json = JsonSerializer.Serialize(createdResult.Value);
            var dict = JsonSerializer.Deserialize<Dictionary<string, Guid>>(json)!;

            Assert.True(dict.ContainsKey("id"));
            Assert.Equal(idEsperado, dict["id"]);
        }


        [Fact]
        public async Task Get_ReturnsListOfReclamos()
        {
            // Arrange
            var reclamos = new List<ReclamoDto>
            {
                new ReclamoDto
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = "Pueba",
                    SubastaId = "Pueba",
                    Motivo = "Motivo 1",
                    Descripcion = "Descripción 1",
                    Estado = "Pendiente",
                    FechaCreacion = DateTime.UtcNow
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObtenerTodosLosReclamosQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(reclamos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedList = Assert.IsType<List<ReclamoDto>>(okResult.Value);
            Assert.Single(returnedList);
        }

        [Fact]
        public async Task ResolverReclamo_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new ReclamoController(mockMediator.Object);
            var reclamoId = Guid.NewGuid();

            mockMediator
                .Setup(m => m.Send(It.IsAny<resolverReclamoCommand>(), default))
                .ReturnsAsync(true);

            // Act
            var result = await controller.ResolverReclamo(reclamoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // 👇 Deserialize el response a diccionario para verificar el campo "mensaje"
            var json = JsonSerializer.Serialize(okResult.Value);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;

            Assert.True(dict.ContainsKey("mensaje"));
            Assert.Equal("Reclamo resuelto correctamente.", dict["mensaje"]);
        }



        [Fact]
        public async Task ResolverReclamo_ThrowsReclamoNoEncontradoException_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<resolverReclamoCommand>(), default))
                .ReturnsAsync(false); // Simula que no se encontró el reclamo

            var controller = new ReclamoController(mediatorMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<reclamoNoEncontradoException>(() =>
                controller.ResolverReclamo(id));

            // En lugar de Assert.Equal, usamos Contains para evitar fallos por formato
            Assert.Contains(id.ToString(), exception.Message);
            Assert.StartsWith("No se encontró el reclamo con ID", exception.Message);
        }

        [Fact]
        public async Task AgregarSolucion_ReclamoExistente_RetornaOk()
        {
            // Arrange
            var dto = new AgregarSolucionReclamoDto
            {
                ReclamoId = Guid.NewGuid(),
                Solucion = "Producto reemplazado por uno nuevo."
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AgregarSolucionReclamoCommand>(), default))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.AgregarSolucion(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            // ✅ Serializar y leer el objeto anónimo con JsonDocument
            var json = JsonSerializer.Serialize(okResult.Value);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Assert.Equal("Solución agregada correctamente al reclamo.", root.GetProperty("mensaje").GetString());
            Assert.Equal(dto.ReclamoId.ToString(), root.GetProperty("reclamoId").GetString());
        }

    }
}
