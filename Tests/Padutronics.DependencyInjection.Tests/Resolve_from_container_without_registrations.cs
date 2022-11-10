using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_without_registrations
{
    private interface IService
    {
    }

    [Test]
    public void Exception_is_thrown_if_service_is_not_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}