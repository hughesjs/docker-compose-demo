using DemoWebApp.Caching;
using DemoWebApp.Common;
using DemoWebApp.Common.Options;
using DemoWebApp.Notes.Models;
using DemoWebApp.Notes.Repositories;
using MongoDB.Driver;
using StackExchange.Redis;

namespace DemoWebApp.Notes.DependencyInjection;

public static class NotesDependencyInjectionExtensions
{
    public static IServiceCollection AddNotes(this IServiceCollection services, ConfigurationManager configuration)
    {
        MongoOptions mongoOptions = configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
        RedisOptions redisOptions = configuration.GetSection(nameof(RedisOptions)).Get<RedisOptions>();
        
        
        services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisOptions.Host));
        services.AddTransient<IMongoDatabase>(_ => new MongoClient(mongoOptions.ConnectionString).GetDatabase(mongoOptions.DatabaseName));
        services.AddTransient<NoteRepository>();
        services.AddTransient<IRepository<Note>, RedisCachedRepository<Note>>(c => new(
            c.GetRequiredService<NoteRepository>(),
            c.GetRequiredService<IConnectionMultiplexer>(),
            c.GetRequiredService<ILogger<RedisCachedRepository<Note>>>()));
        return services;
    }
}