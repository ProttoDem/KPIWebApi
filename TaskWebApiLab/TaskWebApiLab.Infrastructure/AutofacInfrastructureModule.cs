using System.Reflection;
using Ardalis.SharedKernel;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using TaskWebApiLab.Core.GoalAggregate;
using TaskWebApiLab.Core.Interfaces;
using TaskWebApiLab.Core.Services;
using TaskWebApiLab.Infrastructure.Data;
using TaskWebApiLab.UseCases.Goals.Create;
using Module = Autofac.Module;

namespace TaskWebApiLab.Infrastructure
{
    public class AutofacInfrastructureModule : Module
    {
        private readonly bool _isDevelopment = false;
        private readonly List<Assembly> _assemblies = [];

        public AutofacInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
        {
            _isDevelopment = isDevelopment;
            AddToAssembliesIfNotNull(callingAssembly);
        }

        private void AddToAssembliesIfNotNull(Assembly? assembly)
        {
            if (assembly != null)
            {
                _assemblies.Add(assembly);
            }
        }

        private void LoadAssemblies()
        {
            // TODO: Replace these types with any type in the appropriate assembly/project
            var goalAssembly = Assembly.GetAssembly(typeof(Goal));
            var infrastructureAssembly = Assembly.GetAssembly(typeof(AutofacInfrastructureModule));
            var useCasesAssembly = Assembly.GetAssembly(typeof(CreateGoalCommand));

            AddToAssembliesIfNotNull(goalAssembly);
            AddToAssembliesIfNotNull(infrastructureAssembly);
            AddToAssembliesIfNotNull(useCasesAssembly);
        }

        protected override void Load(ContainerBuilder builder)
        {
            LoadAssemblies();
            RegisterEF(builder);
            RegisterMediatR(builder);
        }

        private void RegisterEF(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EfRepository<>))
              .As(typeof(IRepository<>))
              .As(typeof(IReadRepository<>))
              .InstancePerLifetimeScope();
        }

        private void RegisterMediatR(ContainerBuilder builder)
        {
            builder
              .RegisterType<Mediator>()
              .As<IMediator>()
              .InstancePerLifetimeScope();

            builder
              .RegisterGeneric(typeof(LoggingBehavior<,>))
              .As(typeof(IPipelineBehavior<,>))
              .InstancePerLifetimeScope();

            builder
              .RegisterType<MediatRDomainEventDispatcher>()
              .As<IDomainEventDispatcher>()
              .InstancePerLifetimeScope();

            var mediatrOpenTypes = new[]
            {
      typeof(IRequestHandler<,>),
      typeof(IRequestExceptionHandler<,,>),
      typeof(IRequestExceptionAction<,>),
      typeof(INotificationHandler<>),
    };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                  .RegisterAssemblyTypes(_assemblies.ToArray())
                  .AsClosedTypesOf(mediatrOpenType)
                  .AsImplementedInterfaces();
            }
        }

        private void RegisterUserManager(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager<IdentityUser>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserAccessor>()
                .As<IUserAccessor>()
                .InstancePerLifetimeScope();
        }
    }
}
