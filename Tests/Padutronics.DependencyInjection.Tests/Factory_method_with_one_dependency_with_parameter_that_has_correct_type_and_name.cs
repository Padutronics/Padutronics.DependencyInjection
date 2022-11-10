using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Factory_method_with_one_dependency_with_parameter_that_has_correct_type_and_name
{
    public interface IDependency
    {
    }

    private sealed class Dependency : IDependency
    {
    }

    public interface IService
    {
        IDependency Dependency { get; }
    }

    private sealed class Service : IService
    {
        public Service(IDependency dependency)
        {
            Dependency = dependency;
        }

        public IDependency Dependency { get; }
    }

    public interface IServiceFactory
    {
        IService CreateService(IDependency dependency);
    }

    [Test]
    public void Instance_is_created_using_provided_dependency_if_dependency_is_not_registered()
    {
        // Arrange.
        var dependency = new Dependency();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        IService service = serviceFactory.CreateService(dependency);

        // Assert.
        Assert.That(service.Dependency, Is.SameAs(dependency));
    }

    [Test]
    public void Instance_is_created_using_provided_dependency_if_dependency_is_registered()
    {
        // Arrange.
        var dependency1 = new Dependency();
        var dependency2 = new Dependency();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IServiceFactory>().UseFactory();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency>().UseConstant(dependency1);

        using IContainer container = containerBuilder.Build();

        // Act.
        IServiceFactory serviceFactory = container.Resolve<IServiceFactory>();
        IService service = serviceFactory.CreateService(dependency2);

        // Assert.
        Assert.That(service.Dependency, Is.SameAs(dependency2));
    }
}