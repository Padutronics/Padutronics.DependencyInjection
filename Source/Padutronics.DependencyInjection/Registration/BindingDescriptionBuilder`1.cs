using Padutronics.DependencyInjection.Registration.Fluent;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescriptionBuilder<TService> : BindingDescriptionBuilderBase, IBindingStage<TService>
    where TService : class
{
    public BindingDescriptionBuilder() :
        base(typeof(TService))
    {
    }

    public void Use<TImplementation>()
        where TImplementation : class, TService
    {
        Use(typeof(TImplementation));
    }
}