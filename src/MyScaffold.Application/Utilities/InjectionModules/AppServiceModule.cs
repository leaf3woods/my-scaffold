using Autofac;
using MyScaffold.Application.Services.Base;
using MyScaffold.Domain.Services;
using System.Reflection;

namespace MyScaffold.WebApi.Utilities.InjectionModules
{
    public class AppServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("MyScaffold." + nameof(Application)))
                .Where(type => type.IsAssignableTo(typeof(IBaseService)))
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(Assembly.Load("MyScaffold." + nameof(Domain)), Assembly.Load("MyScaffold." + nameof(Infrastructure)))
                .Where(type => type.IsAssignableTo<IDomainService>())
                .AsImplementedInterfaces()
                .PropertiesAutowired();
        }
    }
}