using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Implementation_one_and_two_parameter_constructors_with_the_first_parameter_of_different_type
{
    private enum Constructor
    {
        Unspecified,
        OneParameter,
        TwoParameters
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

    private interface IDependency3
    {
    }

    private sealed class Dependency3 : IDependency3
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
            SelectedConstructor = Constructor.OneParameter;
        }

        public Service(IDependency2 dependency2, IDependency3 dependency3)
        {
            SelectedConstructor = Constructor.TwoParameters;
        }

        public Constructor SelectedConstructor { get; }
    }

    [Test]
    public void One_parameter_constructor_is_selected_if_first_dependency_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency1>().Use<Dependency1>();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service.SelectedConstructor, Is.EqualTo(Constructor.OneParameter));
    }

    [Test]
    public void One_parameter_constructor_is_selected_if_first_and_second_dependency_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency1>().Use<Dependency1>();
        containerBuilder.For<IDependency2>().Use<Dependency2>();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service.SelectedConstructor, Is.EqualTo(Constructor.OneParameter));
    }

    [Test]
    public void One_parameter_constructor_is_selected_if_first_and_third_dependency_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency1>().Use<Dependency1>();
        containerBuilder.For<IDependency3>().Use<Dependency3>();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service.SelectedConstructor, Is.EqualTo(Constructor.OneParameter));
    }

    [Test]
    public void Two_parameter_constructor_is_selected_if_second_and_third_dependency_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency2>().Use<Dependency2>();
        containerBuilder.For<IDependency3>().Use<Dependency3>();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service.SelectedConstructor, Is.EqualTo(Constructor.TwoParameters));
    }

    [Test]
    public void Two_parameter_constructor_is_selected_if_all_dependencies_are_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency1>().Use<Dependency1>();
        containerBuilder.For<IDependency2>().Use<Dependency2>();
        containerBuilder.For<IDependency3>().Use<Dependency3>();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service.SelectedConstructor, Is.EqualTo(Constructor.TwoParameters));
    }

    [Test]
    public void Exception_is_thrown_if_second_dependency_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency2>().Use<Dependency2>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Exception_is_thrown_if_third_dependency_is_registered()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>();
        containerBuilder.For<IDependency3>().Use<Dependency3>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Exception_is_thrown_if_none_of_the_dependencies_are_registered()
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