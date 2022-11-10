using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_container
{
    [Test]
    public void Resolve_the_same_instance_by_container_interface()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        IContainer resolvedContainer = container.Resolve<IContainer>();

        // Assert.
        Assert.That(resolvedContainer, Is.SameAs(container));
    }

    [Test]
    public void Resolve_the_same_instance_by_container_context_interface()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        IContainerContext resolvedContainer = container.Resolve<IContainerContext>();

        // Assert.
        Assert.That(resolvedContainer, Is.SameAs(container));
    }
}