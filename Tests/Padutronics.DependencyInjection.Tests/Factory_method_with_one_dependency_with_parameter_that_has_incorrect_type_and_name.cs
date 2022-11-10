using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Factory_method_with_one_dependency_with_parameter_that_has_incorrect_type_and_name
{
    public interface IDependency1
    {
    }

    private sealed class Dependency1 : IDependency1
    {
    }

    public interface IDependency2
    {
    }

    private sealed class Dependency2 : IDependency2
    {
    }

    public interface IService
    {
        IDependency1 Dependency1 { get; }
    }

    private sealed class Service : IService
    {
        public Service(IDependency1 dependency1)
        {
            Dependency1 = dependency1;
        }

        public IDependency1 Dependency1 { get; }
    }

    public interface IServiceFactory
    {
        IService CreateService(IDependency2 dependency2);
    }

    [Test]
    public void Instance_is_created_using_registered_dependency_if_first_dependency_is_registered()
    {
        // Arrange.
        var dependency1 = new Dependency1();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency1>().UseConstant(dependency1);

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        IService service = serviceFactory.CreateService(new Dependency2());

        // Assert.
        Assert.That(service.Dependency1, Is.SameAs(dependency1));
    }

    [Test]
    public void Instance_is_created_using_registered_dependency_if_both_dependencies_are_registered()
    {
        // Arrange.
        var dependency1 = new Dependency1();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency1>().UseConstant(dependency1);
        containerBuilder.For<IDependency2>().Use<Dependency2>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        IService service = serviceFactory.CreateService(new Dependency2());

        // Assert.
        Assert.That(service.Dependency1, Is.SameAs(dependency1));
    }

    [Test]
    public void Exception_is_thrown_if_none_of_the_dependencies_are_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        void Action() => serviceFactory.CreateService(new Dependency2());

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Exception_is_thrown_if_second_dependency_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency2>().Use<Dependency2>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        void Action() => serviceFactory.CreateService(new Dependency2());

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}