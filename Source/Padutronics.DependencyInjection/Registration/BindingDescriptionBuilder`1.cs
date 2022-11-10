using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution;
using Padutronics.Reflection.Types;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescriptionBuilder<TService> : BindingDescriptionBuilderBase, IFallbackBindingStage<TService>
    where TService : class
{
    public BindingDescriptionBuilder() :
        base(TypeArray.Create<TService>())
    {
    }

    public IBindingStage<TService> IfNone => MarkAsFallbackAndReturn(this);

    public ILifetimeStage Use<TImplementation>()
        where TImplementation : class, TService
    {
        return Use(typeof(TImplementation));
    }

    public IOwnershipStage UseConstant<TImplementation>(TImplementation instance)
        where TImplementation : class, TService
    {
        return UseProvider(new ConstantInstanceProvider<TImplementation>(instance));
    }
}