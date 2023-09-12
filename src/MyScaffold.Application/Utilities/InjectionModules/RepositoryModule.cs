using Autofac;
using System.Reflection;

namespace MyScaffold.WebApi.Utilities.InjectionModules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load(WithPrefix(nameof(Infrastructure))), Assembly.Load(WithPrefix(nameof(Domain))))
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .PropertiesAutowired();
        }

        private static string WithPrefix(string content) => $"MyScaffold." + content;
    }
}