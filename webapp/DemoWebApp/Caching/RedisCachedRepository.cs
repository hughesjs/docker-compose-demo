using System.Text.Json;
using DemoWebApp.Common;
using StackExchange.Redis;

namespace DemoWebApp.Caching;

public class RedisCachedRepository<T> : IRepository<T>
{
    private readonly IRepository<T> _innerRepo;
    private readonly ILogger<RedisCachedRepository<T>> _logger;
    private readonly IDatabaseAsync _redisDb;

    public RedisCachedRepository(IRepository<T> innerRepo, IConnectionMultiplexer redisMultiplexer, ILogger<RedisCachedRepository<T>> logger)
    {
        _innerRepo = innerRepo; 
        _redisDb = redisMultiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<Option<DecoratedEntity<T>>> Get(string id)
    {
        _logger.LogDebug("Trying to get value from cache with id {id}", id);

        RedisValue res = await _redisDb.StringGetAsync(id);

        if (res.IsNullOrEmpty)
        {
            _logger.LogDebug("Cache miss for id {id}, getting from inner repo", id);
            var val = await _innerRepo.Get(id);
            
            if (val.HasSome)
            {
                await _redisDb.StringSetAsync(val.Value!.Id.ToString(), JsonSerializer.Serialize(val.Value), TimeSpan.FromSeconds(30));
            }

            return val;
        }
        
        _logger.LogDebug("Cache hit for id {id}", id);
        return new(JsonSerializer.Deserialize<DecoratedEntity<T>>(res!));
    }


    public async Task<Option<DecoratedEntity<T>>> TrySet(T value)
    {
        var res = await _innerRepo.TrySet(value);
        
        if (res.HasSome)
        {
            _logger.LogInformation("Caching value with id {id} for 30s", res.Value!.Id);
            await _redisDb.StringSetAsync(res.Value.Id.ToString(), JsonSerializer.Serialize(res.Value), TimeSpan.FromSeconds(30));
        }

        return res;
    }
}