using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_with_default_and_fallback_implementations
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
    public void Default_service_is_resolved_if_default_and_fallback_services_are_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service1>();
        containerBuilder.For<IService>().IfNone.Use<Service2>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service, Is.TypeOf<Service1>());
    }

    [Test]
    public void Fallback_service_is_resolved_if_only_fallback_service_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().IfNone.Use<Service1>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service, Is.TypeOf<Service1>());
    }
}