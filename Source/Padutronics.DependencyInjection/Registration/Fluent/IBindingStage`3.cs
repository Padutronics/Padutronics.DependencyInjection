namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IBindingStage<in TService1, in TService2, in TService3>
    where TService1 : class
    where TService2 : class
    where TService3 : class
{
    ILifetimeStage Use<TImplementation>()
        where TImplementation : class, TService1, TService2, TService3;
    IOwnershipStage UseConstant<TImplementation>(TImplementation instance)
        where TImplementation : class, TService1, TService2, TService3;
}