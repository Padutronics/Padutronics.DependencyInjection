using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Can_resolve_container
{
    [Test]
    public void Can_resolve_container_interface()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        bool canResolve = container.CanResolve<IContainer>();

        // Assert.
        Assert.That(canResolve, Is.True);
    }

    [Test]
    public void Can_resolve_container_context_interface()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        bool canResolve = container.CanResolve<IContainerContext>();

        // Assert.
        Assert.That(canResolve, Is.True);
    }
}