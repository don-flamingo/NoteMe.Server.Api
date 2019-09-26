using System.Reflection;
using Autofac;
using AutoMapper;
using NoteMe.Server.Infrastructure.Mappers;

namespace E.Lab.Server.Infrastructure.IoC.Modules
{
    public class MapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(MapperModule)
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