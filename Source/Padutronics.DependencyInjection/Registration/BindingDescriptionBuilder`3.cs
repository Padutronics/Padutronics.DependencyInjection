using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution;
using Padutronics.Reflection.Types;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescriptionBuilder<TService1, TService2, TService3> : BindingDescriptionBuilderBase, IFallbackBindingStage<TService1, TService2, TService3>
    where TService1 : class
    where TService2 : class
    where TService3 : class
{
    public BindingDescriptionBuilder() :
        base(TypeArray.Create<TService1, TService2, TService3>())
    {
    }

    public IBindingStage<TService1, TService2, TService3> IfNone => MarkAsFallbackAndReturn(this);

    public ILifetimeStage Use<TImplementation>()
        where TImplementation : class, TService1, TService2, TService3
    {
        return Use(typeof(TImplementation));
    }

    public IOwnershipStage UseConstant<TImplementation>(TImplementation instance)
        where TImplementation : class, TService1, TService2, TService3
    {
        return UseProvider(new ConstantInstanceProvider<TImplementation>(instance));
    }
}