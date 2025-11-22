using Autofac;
using FormBuilder.Domain;
using FormBuilder.Domain;
using FormBuilder.Domain.Repositories;
using FormBuilder.Infrastructure;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Infrastructure.Repositories;
using MediatR;
using System.Reflection;

namespace FormBuilder.Web
{
    public class WebModule : Autofac.Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public WebModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<FormRepository>().As<IFormRepository>().InstancePerLifetimeScope();

            var applicationAssembly = Assembly.Load("FormBuilder.Application");

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(applicationAssembly)
                   .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}