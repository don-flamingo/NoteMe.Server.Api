using Microsoft.Extensions.Caching.Memory;
using NoteMe.Common.Providers;

namespace NoteMe.Server.Infrastructure.Framework.Cache
{
    public interface ICacheService
    {
        void Set<TEntity>(TEntity entity)
            where TEntity : IIdProvider;

        TEntity Get<TEntity>(object key)
            where TEntity : IIdProvider;
    }

    public class CacheService : ICacheService
    {
        private readonly CacheSettings _cacheSettings;
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache,
            CacheSettings cacheSettings)
        {
            _memoryCache = memoryCache;
            _cacheSettings = cacheSettings;
        }

        public TEntity Get<TEntity>(object key) where TEntity : IIdProvider
            => _memoryCache.Get<TEntity>(GetKey<TEntity>(key));

        public void Set<TEntity>(TEntity entity) where TEntity : IIdProvider
            => _memoryCache.Set(GetKey<TEntity>(entity.Id), entity, _cacheSettings.Duration);

        private string GetKey<TEntity>(object key)
            => $"{typeof(TEntity)}-{key}";
    }
}