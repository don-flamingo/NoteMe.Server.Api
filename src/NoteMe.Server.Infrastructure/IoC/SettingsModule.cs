using Autofac;
using Microsoft.Extensions.Configuration;
using NoteMe.Server.Infrastructure.Extenions;
using NoteMe.Server.Infrastructure.Settings;

namespace NoteMe.Server.Infrastructure.IoC
{
    public class SettingsModule : Module
    {
        private readonly IConfiguration _configuration;

        public SettingsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration.GetSettings<SqlSettings>())
                .SingleInstance();
            builder.RegisterInstance(_configuration.GetSettings<JwtSettings>())
                .SingleInstance();
            builder.RegisterInstance(_configuration.GetSettings<CacheSettings>())
                .SingleInstance();
        }
    }
}