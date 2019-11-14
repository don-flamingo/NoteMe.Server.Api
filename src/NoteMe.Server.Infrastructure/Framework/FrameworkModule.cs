using System.Reflection;
using Autofac;
using AutoMapper;
using NoteMe.Server.Infrastructure.Framework.Mappers;

namespace NoteMe.Server.Infrastructure.Framework
{
    public class FrameworkModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(FrameworkModule)
                .GetTypeInfo()
                .Assembly;
            
            var openTypes = new[]
            {
                typeof(IValueResolver<,,>),
                typeof(IMemberValueResolver<,,,>),
                typeof(ITypeConverter<,>),
                typeof(IValueConverter<,>),
                typeof(IMappingAction<,>)
            };

            foreach (var openType in openTypes)
            {
                builder.RegisterAssemblyTypes(assembly)
                    .AsClosedTypesOf(openType)
                    .InstancePerLifetimeScope();
            }
            
            builder.Register(
                    ctx =>
                    {
                        var scope = ctx.Resolve<ILifetimeScope>();
                        return new Mapper(
                            AutoMapperConfiguration.GetConfiguration(),
                            scope.Resolve);
                    })
                .As<IMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NoteMeMapper>()
                .As<INoteMeMapper>()
                .SingleInstance();
        }
    }
}