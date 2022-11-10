using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_open_generic_that_implements_multiple_services
{
    private interface IService1<T>
    {
    }

    private interface IService2<T>
    {
    }

    private sealed class Service<T> : IService1<T>, IService2<T>
    {
    }

    [Test]
    public void Different_instances_are_resolved_on_each_service_request_when_implementation_is_registered_with_transient_lifetime()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For(new[] { typeof(IService1<>), typeof(IService2<>) }).Use(typeof(Service<>)).InstancePerDependency();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService1<int> service1 = container.Resolve<IService1<int>>();
        IService2<int> service2 = container.Resolve<IService2<int>>();

        // Assert.
        Assert.That(service1, Is.Not.SameAs(service2));
    }

    [Test]
    public void The_same_instance_is_resolved_on_each_service_request_when_implementation_is_registered_with_singleton_lifetime()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For(new[] { typeof(IService1<>), typeof(IService2<>) }).Use(typeof(Service<>)).SingleInstance();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService1<int> service1 = container.Resolve<IService1<int>>();
        IService2<int> service2 = container.Resolve<IService2<int>>();

        // Assert.
        Assert.That(service1, Is.SameAs(service2));
    }
}