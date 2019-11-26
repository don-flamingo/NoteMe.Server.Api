using Autofac;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using NoteMe.Server.Infrastructure.Extenions;

namespace NoteMe.Server.Infrastructure.Cdn
{
    public class CdnModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public CdnModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            var settigns = _configuration.GetSettings<CdnSettings>();
            
            builder.RegisterInstance(settigns)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<CdnService>()
                .As<ICdnService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FileExtensionContentTypeProvider>()
                .AsSelf()
                .SingleInstance();
            
            base.Load(builder);
        }
    }
}