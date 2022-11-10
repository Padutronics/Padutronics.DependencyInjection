using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Can_resolve_from_container_without_registration
{
    private interface IService
    {
    }

    private sealed class Service : IService
    {
    }

    [Test]
    public void Cannot_resolve_if_service_is_not_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        bool canResolve = container.CanResolve<IService>();

        // Assert.
        Assert.That(canResolve, Is.False);
    }
}