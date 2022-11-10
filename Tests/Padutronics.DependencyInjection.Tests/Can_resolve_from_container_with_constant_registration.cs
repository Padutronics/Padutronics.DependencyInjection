using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Can_resolve_from_container_with_constant_registration
{
    private interface IService
    {
    }

    private sealed class Service : IService
    {
    }

    [Test]
    public void Can_resolve_if_constant_is_registered()
    {
        // Arrange.
        var containerBuider = new ContainerBuilder();
        containerBuider.For<IService>().UseConstant(new Service());

        IContainer container = containerBuider.Build();

        // Act.
        bool canResolve = container.CanResolve<IService>();

        // Assert.
        Assert.That(canResolve, Is.True);
    }
}