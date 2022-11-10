using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Implementation_constructor_access
{
    private interface IService
    {
    }

    private sealed class InternalService : IService
    {
        internal InternalService()
        {
        }
    }

    private sealed class PrivateService : IService
    {
        private PrivateService()
        {
        }
    }

    private class PrivateProtectedService : IService
    {
        private protected PrivateProtectedService()
        {
        }
    }

    private class ProtectedService : IService
    {
        protected ProtectedService()
        {
        }
    }

    private class ProtectedInternalService : IService
    {
        protected internal ProtectedInternalService()
        {
        }
    }

    private sealed class PublicService : IService
    {
        public PublicService()
        {
        }
    }

    [Test]
    public void Exception_is_not_thrown_when_implementation_constructor_has_public_access()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<PublicService>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.Nothing);
    }

    [Test]
    public void Exception_is_thrown_when_implementation_constructor_has_internal_access()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<InternalService>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Exception_is_thrown_when_implementation_constructor_has_protected_access()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<ProtectedService>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Exception_is_thrown_when_implementation_constructor_has_protected_internal_access()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<ProtectedInternalService>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Exception_is_thrown_when_implementation_constructor_has_private_access()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<PrivateService>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Exception_is_thrown_when_implementation_constructor_has_private_protected_access()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<PrivateProtectedService>();

        IContainer container = containerBuilder.Build();

        // Act.
        void Action() => container.Resolve<IService>();

        // Assert.
        Assert.That(Action, Throws.TypeOf<InvalidOperationException>());
    }
}