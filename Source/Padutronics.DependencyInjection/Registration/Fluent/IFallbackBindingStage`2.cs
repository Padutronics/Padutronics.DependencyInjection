namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IFallbackBindingStage<in TService1, in TService2> : IBindingStage<TService1, TService2>
    where TService1 : class
    where TService2 : class
{
    IBindingStage<TService1, TService2> IfNone { get; }
}