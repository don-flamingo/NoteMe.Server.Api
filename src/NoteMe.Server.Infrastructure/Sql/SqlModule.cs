using Autofac;
using Microsoft.Extensions.Configuration;
using NoteMe.Server.Infrastructure.Extenions;

namespace NoteMe.Server.Infrastructure.Sql
{
    public class SqlModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public SqlModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var settings = _configuration.GetSettings<SqlSettings>();
            builder.RegisterInstance(settings)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<NoteMeContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}