using Moq;
using NUnit.Framework;
using System;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Disposing
{
    public interface IService : IDisposable
    {
    }

    [Test]
    public void Instance_is_disposed_during_container_disposing_if_it_is_owned_by_container()
    {
        // Arrange.
        var serviceMock = new Mock<IService>();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().UseConstant(serviceMock.Object).OwnedByContainer();

        IService service;

        using (IContainer container = containerBuilder.Build())
        {
            service = container.Resolve<IService>();

            // Act.
        }

        // Assert.
        serviceMock.Verify(mock => mock.Dispose(), Times.AtLeastOnce());
    }

    [Test]
    public void Instance_is_not_disposed_during_container_disposing_if_it_is_not_owned_by_container()
    {
        // Arrange.
        var serviceMock = new Mock<IService>();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().UseConstant(serviceMock.Object).ExternallyOwned();

        IService service;

        using (IContainer container = containerBuilder.Build())
        {
            service = container.Resolve<IService>();

            // Act.
        }

        // Assert.
        serviceMock.Verify(mock => mock.Dispose(), Times.Never());
    }
}