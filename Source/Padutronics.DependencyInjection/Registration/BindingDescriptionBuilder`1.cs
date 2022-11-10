using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescriptionBuilder<TService> : BindingDescriptionBuilderBase, IBindingStage<TService>
    where TService : class
{
    public BindingDescriptionBuilder() :
        base(typeof(TService))
    {
    }

    public ILifetimeStage Use<TImplementation>()
        where TImplementation : class, TService
    {
        return Use(typeof(TImplementation));
    }

    public void UseConstant<TImplementation>(TImplementation instance)
        where TImplementation : class, TService
    {
        UseProvider(new ConstantInstanceProvider<TImplementation>(instance));
    }
}