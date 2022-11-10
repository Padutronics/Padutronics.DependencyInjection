using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Factory_open_generic_service
{
    public interface IService<T>
    {
    }

    private sealed class Service<T> : IService<T>
    {
    }

    public interface IServiceFactory
    {
        IService<T> CreateService<T>();
    }

    [Test]
    public void Instance_is_created_using_container_registration()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For(typeof(IService<>)).Use(typeof(Service<>));

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        IService<int> service = serviceFactory.CreateService<int>();

        // Assert.
        Assert.That(service, Is.TypeOf<Service<int>>());
    }
}