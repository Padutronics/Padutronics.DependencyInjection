using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_wth_incomplete_registration
{
    private interface IService
    {
    }

    [Test]
    public void Exception_is_thrown_if_implementation_is_not_specified()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>();

        // Act.
        void Action() => containerBuilder.Build();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}