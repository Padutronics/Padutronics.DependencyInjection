using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_with_multiple_default_and_fallback_implementations
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
    public void Last_registered_fallback_service_is_resolved_if_multiple_fallback_services_are_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().IfNone.Use<Service1>();
        containerBuilder.For<IService>().IfNone.Use<Service2>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service, Is.TypeOf<Service2>());
    }

    [Test]
    public void Last_registered_default_service_is_resolved_if_multiple_default_services_are_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service1>();
        containerBuilder.For<IService>().Use<Service2>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service, Is.TypeOf<Service2>());
    }
}