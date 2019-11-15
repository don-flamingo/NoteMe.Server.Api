using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NoteMe.Server.Infrastructure.Extenions;
using NoteMe.Server.Infrastructure.Framework.Cache;
using NoteMe.Server.Infrastructure.Framework.Generators;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Framework.Security;

namespace NoteMe.Server.Infrastructure.Framework
{
    public class FrameworkModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public FrameworkModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            var mapperConfiguration = AutoMapperConfiguration.GetConfiguration();
            var mapper = mapperConfiguration.CreateMapper();

            builder.RegisterInstance(mapper)
                .As<IMapper>()
                .SingleInstance();

            builder.RegisterType<NoteMeMapper>()
                .As<INoteMeMapper>()
                .SingleInstance();

            builder.RegisterType<MemoryCache>()
                .As<IMemoryCache>()
                .SingleInstance();

            builder.RegisterType<CacheService>()
                .As<ICacheService>()
                .SingleInstance();

            builder.RegisterType<SecurityService>()
                .As<ISecurityService>()
                .SingleInstance();

            var cacheSettings = _configuration.GetSettings<CacheSettings>();
            var securitySettings = _configuration.GetSettings<SecuritySettings>();

            builder.RegisterInstance(cacheSettings)
                .AsSelf()
                .SingleInstance();

            builder.RegisterInstance(securitySettings)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<DataSeeder>()
                .As<IDataSeeder>()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(FrameworkModule).Assembly)
                .AsClosedTypesOf(typeof(IDataSeeder<>))
                .InstancePerDependency();
        }
    }
}