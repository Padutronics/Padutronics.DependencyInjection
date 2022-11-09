using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_with_default_type_registration
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
        var containerBuider = new ContainerBuilder();
        containerBuider.For<IService>().Use<Service>();

        IContainer container = containerBuider.Build();

        // Act.
        IService service = container.Resolve<IService>();

        // Assert.
        Assert.That(service, Is.TypeOf<Service>());
    }
}