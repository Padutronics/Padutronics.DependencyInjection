namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IBindingStage<in TService>
    where TService : class
{
    void Use<TImplementation>()
        where TImplementation : class, TService;
}