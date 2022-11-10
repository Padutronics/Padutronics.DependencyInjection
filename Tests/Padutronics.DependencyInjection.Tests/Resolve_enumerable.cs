using NUnit.Framework;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_enumerable
{
    private interface IService
    {
    }

    private sealed class Service1 : IService
    {
    }

    private sealed class Service2 : IService
    {
    }

    [Test]
    public void All_registered_instances_are_resolved_if_services_are_registered()
    {
        // Arrange.
        var service1 = new Service1();
        var service2 = new Service2();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().UseConstant(service1);
        containerBuilder.For<IService>().UseConstant(service2);

        using IContainer container = containerBuilder.Build();

        // Act.
        IEnumerable<IService> services = container.Resolve<IEnumerable<IService>>();

        // Assert.
        Assert.That(services, Is.EquivalentTo(new IService[] { service1, service2 }));
    }

    [Test]
    public void Empty_enumeration_is_resolved_if_services_are_not_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        IEnumerable<IService> services = container.Resolve<IEnumerable<IService>>();

        // Assert.
        Assert.That(services, Is.Empty);
    }
}