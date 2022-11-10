namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IFallbackBindingStage<in TService> : IBindingStage<TService>
    where TService : class
{
    IBindingStage<TService> IfNone { get; }
}