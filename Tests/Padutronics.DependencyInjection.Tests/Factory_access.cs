using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Factory_access
{
    private interface IService
    {
    }

    private sealed class Service : IService
    {
    }

    private interface IServiceFactory
    {
        IService CreateService();
    }

    [Test]
    public void Exception_is_thrown_if_factory_interface_is_inaccessible()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IServiceFactory>().UseFactory();

        using IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IServiceFactory>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<ArgumentException>());
    }
}