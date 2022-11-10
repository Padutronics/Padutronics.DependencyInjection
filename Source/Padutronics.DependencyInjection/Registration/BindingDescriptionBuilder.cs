using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution;
using System;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescriptionBuilder : BindingDescriptionBuilderBase, IBindingStage
{
    public BindingDescriptionBuilder(Type serviceType) :
        base(serviceType)
    {
    }

    public void UseConstant(object instance)
    {
        UseProvider(new ConstantInstanceProvider<object>(instance));
    }
}