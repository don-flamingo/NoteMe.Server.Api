using System;
using Microsoft.Extensions.Caching.Memory;
using NoteMe.Common.Dtos;
using NoteMe.Server.Infrastructure.Services.Common;
using NoteMe.Server.Infrastructure.Settings;

namespace NoteMe.Server.Infrastructure.Services
{
    public interface IMemoryCacheService : IService, IDisposable
    {
        void SetJwt(Guid tokenGuid, JwtDto jwt);
        JwtDto GetJwt(Guid tokenGuid);
    }
    
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly CacheSettings _cacheSettings;
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(
            CacheSettings cacheSettings,
            IMemoryCache memoryCache)
        {
            _cacheSettings = cacheSettings;
            _memoryCache = memoryCache;
        }
        
        public void SetJwt(Guid tokenGuid, JwtDto jwt)
            => _memoryCache.Set(GetJwtKey(tokenGuid), jwt, _cacheSettings.Interval);

        public JwtDto GetJwt(Guid tokenGuid)
            => _memoryCache.Get<JwtDto>(GetJwtKey(tokenGuid));
        
        private static string GetJwtKey(Guid tokenGuid)
            => $"jwt-{tokenGuid}";
        
        public void Dispose()
        {
            _memoryCache?.Dispose();
        }
    }
}