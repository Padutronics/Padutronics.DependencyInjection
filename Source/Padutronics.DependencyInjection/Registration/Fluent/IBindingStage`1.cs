namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IBindingStage<in TService>
    where TService : class
{
    ILifetimeStage Use<TImplementation>()
        where TImplementation : class, TService;
}