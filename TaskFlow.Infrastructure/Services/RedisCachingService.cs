using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Infrastructure.Configurations;

namespace TaskFlow.Infrastructure.Services
{
    public class RedisCachingService : ICachingService
    {
        private readonly IDatabase _database;
        private readonly TimeSpan _time;
        public RedisCachingService(IConnectionMultiplexer multiplexer, IOptions<CacheTimeSet> options)
        {
            _database = multiplexer.GetDatabase();
            var minutes = options.Value.CacheExpirationMinutes;
            _time = TimeSpan.FromMinutes(minutes);
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await _database.StringGetAsync(key);
                if (value.IsNullOrEmpty)
                {
                    return default(T?);
                }
                return JsonSerializer.Deserialize<T>(value);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("RedisCache Sevice: GetAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("RedisCache Service: Problem with GetAsync");
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _database.KeyDeleteAsync(key);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("RedisCache Sevice: RemoveAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("RedisCache Service: Problem with RemoveAsync");
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? exp)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                await _database.StringSetAsync(key, json, exp ?? _time);
            }
            catch (OperationCanceledException oex)
            {
                throw new Exception("RedisCache Sevice: SetAsync operation were canceled");
            }
            catch (Exception ex)
            {
                throw new Exception("RedisCache Service: Problem with SetAsync");
            }
        }
    }
}
