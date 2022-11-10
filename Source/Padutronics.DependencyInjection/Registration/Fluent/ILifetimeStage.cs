namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface ILifetimeStage
{
    void InstancePerDependency();
    IOwnershipStage SingleInstance();
}