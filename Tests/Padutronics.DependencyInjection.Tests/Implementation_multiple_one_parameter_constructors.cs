using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Implementation_multiple_one_parameter_constructors
{
    private enum Constructor
    {
        Unspecified,
        OneParameter1,
        OneParameter2
    }

    private interface IDependency1
    {
    }

    private sealed class Dependency1 : IDependency1
    {
    }

    private interface IDependency2
    {
    }

    private sealed class Dependency2 : IDependency2
    {
    }

    private interface IService
    {
        Constructor SelectedConstructor { get; }
    }

    private sealed class Service : IService
    {
        public Service(IDependency1 dependency1)
        {
            SelectedConstructor = Constructor.OneParameter1;
        }

        public Service(IDependency2 dependency2)
        {
            SelectedConstructor = Constructor.OneParameter2;
        }

        public Constructor SelectedConstructor { get; }
    }

    [Test]
    public void The_first_constructor_with_one_parameter_is_selected_if_dependency_for_the_first_constructor_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency1>().Use<Dependency1>();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service.SelectedConstructor, Is.EqualTo(Constructor.OneParameter1));
    }

    [Test]
    public void The_second_constructor_with_one_parameter_is_selected_if_dependency_for_the_second_constructor_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency2>().Use<Dependency2>();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service.SelectedConstructor, Is.EqualTo(Constructor.OneParameter2));
    }

    [Test]
    public void Exception_is_thrown_if_none_of_dependencies_are_registered()
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

    [Test]
    public void Exception_is_thrown_if_both_dependencies_are_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency1>().Use<Dependency1>();
        containerBuilder.For<IDependency2>().Use<Dependency2>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}