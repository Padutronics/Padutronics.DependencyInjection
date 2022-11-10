using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Factory_method_with_one_dependency_without_parameters
{
    public interface IDependency
    {
    }

    private sealed class Dependency : IDependency
    {
    }

    public interface IService
    {
        IDependency Dependency { get; }
    }

    private sealed class Service : IService
    {
        public Service(IDependency dependency)
        {
            Dependency = dependency;
        }

        public IDependency Dependency { get; }
    }

    public interface IServiceFactory
    {
        IService CreateService();
    }

    [Test]
    public void Instance_is_created_using_container_registration()
    {
        // Arrange.
        var dependency = new Dependency();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency>().UseConstant(dependency);

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        IService service = serviceFactory.CreateService();

        // Assert.
        Assert.That(service.Dependency, Is.SameAs(dependency));
    }

    [Test]
    public void Exception_is_thrown_if_service_dependency_is_not_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        void Action() => serviceFactory.CreateService();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}