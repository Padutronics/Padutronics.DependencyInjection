using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_lazy
{
    private interface IService
    {
    }

    private sealed class Service : IService
    {
    }

    [Test]
    public void Lazy_instance_is_resolved_if_service_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();

        using IContainer container = containerBuilder.Build();

        // Act.
        Lazy<IService> lazyService = container.Resolve<Lazy<IService>>();

        // Assert.
        Assert.That(lazyService, Is.Not.Null);
    }

    [Test]
    public void Lazy_instance_is_resolved_if_service_is_not_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        Lazy<IService> lazyService = container.Resolve<Lazy<IService>>();

        // Assert.
        Assert.That(lazyService, Is.Not.Null);
    }

    [Test]
    public void Instance_is_resolved_when_lazy_object_is_used_if_service_is_registered()
    {
        // Arrange.
        var expectedService = new Service();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().UseConstant(expectedService);

        using IContainer container = containerBuilder.Build();

        // Act.
        Lazy<IService> lazyService = container.Resolve<Lazy<IService>>();
        IService actualService = lazyService.Value;

        // Assert.
        Assert.That(actualService, Is.SameAs(expectedService));
    }

    [Test]
    public void Exception_is_thrown_when_lazy_object_is_used_if_service_is_not_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();

        using IContainer container = containerBuilder.Build();

        // Act.
        Lazy<IService> lazyService = container.Resolve<Lazy<IService>>();
        void Action()
        {
            IService actualService = lazyService.Value;
        }

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}