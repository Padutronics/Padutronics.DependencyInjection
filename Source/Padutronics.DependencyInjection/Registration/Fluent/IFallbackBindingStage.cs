namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IFallbackBindingStage : IBindingStage
{
    IBindingStage IfNone { get; }
}