using Autofac;
using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Fee.Services.Applying.API.Application.Commands;
using Microsoft.Fee.Services.Applying.API.Application.Queries;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
using Microsoft.Fee.Services.Applying.Infrastructure.Idempotency;
using Microsoft.Fee.Services.Applying.Infrastructure.Repositories;
using System.Reflection;

namespace Microsoft.Fee.Services.Applying.API.Infrastructure.AutofacModules
{

    public class ApplicationModule : Autofac.Module
    {

        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;

        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => new ApplicationQueries(QueriesConnectionString))
                .As<IApplicationQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StudentRepository>()
                .As<IStudentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationRepository>()
                .As<IApplicationRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
               .As<IRequestManager>()
               .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CreateApplicationCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

        }
    }
}
