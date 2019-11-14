using Autofac;
using Microsoft.Extensions.Configuration;
using NoteMe.Server.Infrastructure.Cqrs;
using NoteMe.Server.Infrastructure.Framework;

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
            builder.RegisterModule(new CqrsModule());
            builder.RegisterModule(new FrameworkModule(_configuration));
        }
    }
}