using DemoWebApp.Notes.DependencyInjection;
using DemoWebApp.Notes.WebApplication;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddNotes();

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapNotes();

await app.RunAsync();