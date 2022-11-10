using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_with_default_constant_registration
{
    private interface IService
    {
    }

    private sealed class Service : IService
    {
    }

    [Test]
    public void Instance_of_registered_implementation_type_is_resolved()
    {
        // Arrange.
        var expectedService = new Service();

        var containerBuider = new ContainerBuilder();
        containerBuider.For<IService>().UseConstant(expectedService);

        IContainer container = containerBuider.Build();

        // Act.
        IService actualService = container.Resolve<IService>();

        // Assert.
        Assert.That(actualService, Is.SameAs(expectedService));
    }
}