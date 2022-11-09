using Padutronics.DependencyInjection.Registration.Fluent;
using System;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescriptionBuilder : BindingDescriptionBuilderBase, IBindingStage
{
    public BindingDescriptionBuilder(Type serviceType) :
        base(serviceType)
    {
    }
}