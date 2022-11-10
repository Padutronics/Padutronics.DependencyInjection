namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IOwnershipStage
{
    void ExternallyOwned();
    void OwnedByContainer();
}