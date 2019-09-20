using Autofac;
using NoteMe.Server.Infrastructure.Services;
using NoteMe.Server.Infrastructure.Services.Common;

namespace NoteMe.Server.Infrastructure.IoC
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ServiceModule).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsAssignableTo<IService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<IMemoryCacheService>()
                .As<MemoryCacheService>()
                .SingleInstance();
        }
    }
}