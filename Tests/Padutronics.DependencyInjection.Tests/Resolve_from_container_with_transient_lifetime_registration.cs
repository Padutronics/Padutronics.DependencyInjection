using NUnit.Framework;

namespace Padutronics.DependencyInjection.Tests;

[TestFixture]
internal sealed class Resolve_from_container_with_transient_lifetime_registration
{
    private interface IService
    {
    }

    private sealed class Service : IService
    {
    }

    [Test]
    public void Different_instance_is_resolved_on_each_request()
    {
        // Arrange.
        var containerBuilder = new ContainerBuilder();
        containerBuilder.For<IService>().Use<Service>().InstancePerDependency();

        IContainer container = containerBuilder.Build();

        // Act.
        IService service1 = container.Resolve<IService>();
        IService service2 = container.Resolve<IService>();

        // Assert.
        Assert.That(service1, Is.Not.SameAs(service2));
    }
}