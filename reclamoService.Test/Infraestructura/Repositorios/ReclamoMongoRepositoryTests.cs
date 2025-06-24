using Mongo2Go;
using MongoDB.Driver;
using reclamoService.Dominio.Entidades;
using reclamoService.Infraestructura.Mongo;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ReclamoMongoRepositoryTests : IDisposable
{
    private readonly MongoDbRunner _runner;
    private readonly IMongoDatabase _database;
    private readonly ReclamoMongoRepository _repo;

    public ReclamoMongoRepositoryTests()
    {
        _runner = MongoDbRunner.Start();
        var client = new MongoClient(_runner.ConnectionString);
        _database = client.GetDatabase("TestDb");
        _repo = new ReclamoMongoRepository(_database);
    }

    public void Dispose()
    {
        _runner.Dispose();
    }

    [Fact]
    public async Task GuardarAsync_Should_InsertReclamo()
    {
        // Arrange
        var reclamo = new reclamo
        {
            Id = Guid.NewGuid(),
            UsuarioId = "user123",
            SubastaId = "auction456",
            Motivo = "Producto dañado",
            Descripcion = "El artículo llegó roto",
            FechaCreacion = DateTime.UtcNow,
            Estado = "Pendiente"
        };

        // Act
        await _repo.GuardarAsync(reclamo);
        var result = await _database.GetCollection<reclamo>("Reclamos").Find(r => r.Id == reclamo.Id).FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("user123", result.UsuarioId);
    }

    [Fact]
    public async Task ObtenerTodosAsync_Should_ReturnAllReclamos()
    {
        // Arrange
        var reclamo1 = new reclamo { Id = Guid.NewGuid(), UsuarioId = "1", Estado = "Pendiente", FechaCreacion = DateTime.UtcNow };
        var reclamo2 = new reclamo { Id = Guid.NewGuid(), UsuarioId = "2", Estado = "Resuelto", FechaCreacion = DateTime.UtcNow };
        await _repo.GuardarAsync(reclamo1);
        await _repo.GuardarAsync(reclamo2);

        // Act
        var result = await _repo.ObtenerTodosAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ActualizarEstadoAsync_Should_UpdateEstado()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reclamo = new reclamo
        {
            Id = id,
            UsuarioId = "user123",
            Estado = "Pendiente",
            FechaCreacion = DateTime.UtcNow
        };
        await _repo.GuardarAsync(reclamo);

        // Act
        await _repo.ActualizarEstadoAsync(id, "Resuelto");
        var updated = await _database.GetCollection<reclamo>("Reclamos").Find(r => r.Id == id).FirstOrDefaultAsync();

        // Assert
        Assert.Equal("Resuelto", updated.Estado);
    }
}
