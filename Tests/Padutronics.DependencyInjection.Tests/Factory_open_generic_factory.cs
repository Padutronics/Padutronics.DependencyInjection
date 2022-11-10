using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Factory_open_generic_factory
{
    public interface IService
    {
    }

    private sealed class Service : IService
    {
    }

    public interface IServiceFactory<T>
    {
        T CreateService();
    }

    [Test]
    public void Instance_is_created_using_container_registration()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For(typeof(IServiceFactory<>)).UseFactory();
        containerBuilder.For<IService>().Use<Service>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory<IService> serviceFactory = container.Resolve<IServiceFactory<IService>>();
        IService service = serviceFactory.CreateService();

        // Assert.
        Assert.That(service, Is.TypeOf<Service>());
    }
}