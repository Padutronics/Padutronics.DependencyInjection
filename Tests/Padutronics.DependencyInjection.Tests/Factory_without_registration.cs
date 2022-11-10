using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Factory_without_registration
{
    public interface IService
    {
    }

    public interface IServiceFactory
    {
        IService CreateService();
    }

    [Test]
    public void Exception_is_thrown_is_service_is_not_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        void Action() => serviceFactory.CreateService();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}