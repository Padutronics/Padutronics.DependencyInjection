namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IBindingStage<in TService1, in TService2>
    where TService1 : class
    where TService2 : class
{
    ILifetimeStage Use<TImplementation>()
        where TImplementation : class, TService1, TService2;
    IOwnershipStage UseConstant<TImplementation>(TImplementation instance)
        where TImplementation : class, TService1, TService2;
}