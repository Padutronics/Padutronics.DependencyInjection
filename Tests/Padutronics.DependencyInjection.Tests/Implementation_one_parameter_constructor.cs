using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Implementation_one_parameter_constructor
{
    private interface IDependency
    {
    }

    private sealed class Dependency : IDependency
    {
    }

    private interface IService
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

    [Test]
    public void Dependency_is_injected_if_it_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency>().Use<Dependency>();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service.Dependency, Is.Not.Null);
    }

    [Test]
    public void Exception_is_thrown_if_dependency_is_not_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}