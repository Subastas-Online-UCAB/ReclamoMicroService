using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using reclamoService.Aplicacion.Commands;
using reclamoService.Dominio.Interfaces;
using reclamoService.Infraestructura.Persistencia;
using reclamoService.Infraestructura.Repositorios;
using reclamoService.Infraestructura.Consumers;
using reclamoService.Infraestructura.Mongo;
using reclamoService.Infraestructura.EventPublisher;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;


// Solución al error de serialización de GUID
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// Servicios del contenedor
// -------------------------------

// Controladores
builder.Services.AddControllers();

// Swagger + Comentarios XML
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// -------------------------------
// Base de datos PostgreSQL
// -------------------------------
builder.Services.AddDbContext<ReclamoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

// -------------------------------
// Base de datos MongoDB
// -------------------------------
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetSection("MongoDb")["ConnectionString"];
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var dbName = builder.Configuration.GetSection("MongoDb")["Database"];
    return client.GetDatabase(dbName);
});

builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddScoped<IReclamoMongoRepository, ReclamoMongoRepository>();

// -------------------------------
// Repositorios y Servicios
// -------------------------------
builder.Services.AddScoped<IReclamoRepository, ReclamoRepository>();
builder.Services.AddScoped<IReclamoEventPublisher, ReclamoEventPublisher>();

// -------------------------------
// MediatR
// -------------------------------
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(createReclamoCommand).Assembly));

// -------------------------------
// MassTransit + RabbitMQ
// -------------------------------
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReclamoCreadoConsumer>();
    x.AddConsumer<ReclamoResueltoConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("cola-reclamos-creados", e =>
        {
            e.ConfigureConsumer<ReclamoCreadoConsumer>(context);
        });

        cfg.ReceiveEndpoint("cola-reclamos-resueltos", e =>
        {
            e.ConfigureConsumer<ReclamoResueltoConsumer>(context);
        });
    });
});

// -------------------------------
// Aplicación
// -------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
