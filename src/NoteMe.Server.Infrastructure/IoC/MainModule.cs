using Autofac;
using E.Lab.Server.Infrastructure.IoC.Modules;
using Microsoft.Extensions.Configuration;

namespace NoteMe.Server.Infrastructure.IoC
{
    public class MainModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public MainModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new SettingsModule(_configuration));
            builder.RegisterModule(new CqrsModule());
            builder.RegisterModule(new MapperModule());
            builder.RegisterModule(new ServiceModule());
        }
    }
}