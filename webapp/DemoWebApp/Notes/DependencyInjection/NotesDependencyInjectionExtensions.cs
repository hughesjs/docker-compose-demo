using DemoWebApp.Caching;
using DemoWebApp.Common;
using DemoWebApp.Notes.Models;
using DemoWebApp.Notes.Repositories;
using MongoDB.Driver;
using StackExchange.Redis;

namespace DemoWebApp.Notes.DependencyInjection;

public static class NotesDependencyInjectionExtensions
{
    public static IServiceCollection AddNotes(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("localhost"));
        services.AddTransient<IMongoClient>(_ => new MongoClient("mongodb://localhost:27017"));
        services.AddTransient<NoteRepository>();
        services.AddTransient<IRepository<Note>, RedisCachedRepository<Note>>(c => new(
            c.GetRequiredService<NoteRepository>(),
            c.GetRequiredService<IConnectionMultiplexer>(),
            c.GetRequiredService<ILogger<RedisCachedRepository<Note>>>()));
        return services;
    }
}