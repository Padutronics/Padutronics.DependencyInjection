using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_with_registration_that_implements_multiple_services
{
    private interface IService1
    {
    }

    private interface IService2
    {
    }

    private sealed class Service : IService1, IService2
    {
    }

    [Test]
    public void Different_instances_are_resolved_on_each_service_request_when_implementation_is_registered_with_transient_lifetime()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService1, IService2>().Use<Service>().InstancePerDependency();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService1 service1 = container.Resolve<IService1>();
        IService2 service2 = container.Resolve<IService2>();

        // Assert.
        Assert.That(service1, Is.Not.SameAs(service2));
    }

    [Test]
    public void The_same_instance_is_resolved_on_each_service_request_when_implementation_is_registered_with_singleton_lifetime()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService1, IService2>().Use<Service>().SingleInstance();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService1 service1 = container.Resolve<IService1>();
        IService2 service2 = container.Resolve<IService2>();

        // Assert.
        Assert.That(service1, Is.SameAs(service2));
    }
}