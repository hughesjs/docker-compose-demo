using DemoWebApp.Notes.DependencyInjection;
using DemoWebApp.Notes.WebApplication;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false)
                     .AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddNotes(builder.Configuration);

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapNotes();

await app.RunAsync();