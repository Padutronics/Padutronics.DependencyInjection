using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Factory_method_with_two_dependencies_with_parameters_in_incorrect_order
{
    public interface IDependency1
    {
    }

    private sealed class Dependency1 : IDependency1
    {
    }

    public interface IDependency2
    {
    }

    private sealed class Dependency2 : IDependency2
    {
    }

    public interface IService
    {
        IDependency1 Dependency1 { get; }
        IDependency2 Dependency2 { get; }
    }

    private sealed class Service : IService
    {
        public Service(IDependency1 dependency1, IDependency2 dependency2)
        {
            Dependency1 = dependency1;
            Dependency2 = dependency2;
        }

        public IDependency1 Dependency1 { get; }

        public IDependency2 Dependency2 { get; }
    }

    public interface IServiceFactory
    {
        IService CreateService(IDependency2 dependency2, IDependency1 dependency1);
    }

    [Test]
    public void Instance_is_created_using_provided_dependencies()
    {
        // Arrange.
        var dependency1 = new Dependency1();
        var dependency2 = new Dependency2();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        IService service = serviceFactory.CreateService(dependency2, dependency1);

        // Assert.
        Assert.That(service.Dependency1, Is.SameAs(dependency1));
        Assert.That(service.Dependency2, Is.SameAs(dependency2));
    }
}