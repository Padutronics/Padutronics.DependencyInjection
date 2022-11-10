using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_with_singleton_lifetime_registration
{
    private interface IService
    {
    }

    private sealed class Service : IService
    {
    }

    [Test]
    public void The_same_instance_is_resolved_on_each_request()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>().SingleInstance();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service1 = container.Resolve<IService>();
        IService service2 = container.Resolve<IService>();

        // Assert.
        Assert.That(service1, Is.SameAs(service2));
    }
}