namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IFallbackBindingStage<in TService1, in TService2, in TService3> : IBindingStage<TService1, TService2, TService3>
    where TService1 : class
    where TService2 : class
    where TService3 : class
{
    IBindingStage<TService1, TService2, TService3> IfNone { get; }
}