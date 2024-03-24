using DemoWebApp.Caching;
using DemoWebApp.Common;
using DemoWebApp.Notes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect("localhost"));
builder.Services.AddTransient<IMongoClient>(_ => new MongoClient("mongodb://localhost:27017"));
builder.Services.AddTransient<NoteRepository>();

//MS DI can't handle decorating very well...
builder.Services.AddTransient<IRepository<Note>, RedisCachedRepository<Note>>(c => new(
    c.GetRequiredService<NoteRepository>(),
    c.GetRequiredService<IConnectionMultiplexer>(),
    c.GetRequiredService<ILogger<RedisCachedRepository<Note>>>()));

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/notes/{id}", async ([FromServices] IRepository<Note> notesRepo, [FromRoute] string id) =>
    {
        var option = await notesRepo.Get(id);

        return option.HasSome ? Results.Ok(option.Value) : Results.NotFound();
    })
    .WithName("GetNotes")
    .WithOpenApi();

app.MapPost("/notes", async ([FromServices] IRepository<Note> notesRepo, [FromBody] Note note) =>
{
    var option = await notesRepo.TrySet(note);

    return option.HasSome ? Results.Created($"/notes/{option.Value!.Id}", option.Value) : Results.Problem();
});

app.Run();