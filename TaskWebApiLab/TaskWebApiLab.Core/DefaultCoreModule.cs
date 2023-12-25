using Autofac;
using TaskWebApiLab.Core.Interfaces;
using TaskWebApiLab.Core.Services;

namespace TaskWebApiLab.Core
{
    public class DefaultCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserAccessor>().As<IUserAccessor>().InstancePerLifetimeScope();
        }
    }
}
