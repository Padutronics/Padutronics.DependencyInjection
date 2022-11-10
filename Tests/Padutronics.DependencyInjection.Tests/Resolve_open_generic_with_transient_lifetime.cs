using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_open_generic_with_transient_lifetime
{
    private interface IService<T>
    {
    }

    private sealed class Service<T> : IService<T>
    {
    }

    [Test]
    public void Different_instances_are_resolved_on_each_service_request_of_different_type_when_implementation_is_registered_with_transient_lifetime()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For(typeof(IService<>)).Use(typeof(Service<>)).InstancePerDependency();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService<int> service1 = container.Resolve<IService<int>>();
        IService<string> service2 = container.Resolve<IService<string>>();

        // Assert.
        Assert.That(service1, Is.Not.Null);
        Assert.That(service2, Is.Not.Null);
        Assert.That(service1, Is.Not.SameAs(service2));
    }

    [Test]
    public void Different_instances_are_resolved_on_each_service_request_of_the_same_type_when_implementation_is_registered_with_transient_lifetime()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For(typeof(IService<>)).Use(typeof(Service<>)).InstancePerDependency();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService<int> service1 = container.Resolve<IService<int>>();
        IService<int> service2 = container.Resolve<IService<int>>();

        // Assert.
        Assert.That(service1, Is.Not.Null);
        Assert.That(service2, Is.Not.Null);
        Assert.That(service1, Is.Not.SameAs(service2));
    }
}