using System;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using NoteMe.Common.DataTypes.Providers;
using NoteMe.Common.Dtos;
using NoteMe.Server.Infrastructure.Services.Common;
using NoteMe.Server.Infrastructure.Settings;

namespace NoteMe.Server.Infrastructure.Services
{
    public interface IMemoryCacheService : IService, IDisposable
    {
        void SetJwt(Guid tokenGuid, JwtDto jwt);
        JwtDto GetJwt(Guid tokenGuid);
        void SetDto<TDto>(TDto dto) where TDto : IDtoProvider, IIdProvider;
        TDto GetDto<TDto>(Guid id) where TDto : IDtoProvider;
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
        
        public void SetDto<TDto>(TDto dto) where TDto : IDtoProvider, IIdProvider
            => _memoryCache.Set(GetDtoKey<TDto>(dto.Id), dto, _cacheSettings.Interval);
        public TDto GetDto<TDto>(Guid id) where TDto : IDtoProvider
            => _memoryCache.Get<TDto>(GetDtoKey<TDto>(id));
        private string GetDtoKey<TDto>(Guid id) where TDto : IDtoProvider
            => $"dto-{RefactorDtoTypeName(typeof(TDto))}-{id}";
        private string RefactorDtoTypeName(MemberInfo dtoType)
            => dtoType.Name.Remove(dtoType.Name.Length - 3, 3).ToLower();
        
        public void Dispose()
        {
            _memoryCache?.Dispose();
        }
    }
}