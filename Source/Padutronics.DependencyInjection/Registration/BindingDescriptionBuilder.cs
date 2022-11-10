using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescriptionBuilder : BindingDescriptionBuilderBase, IFallbackBindingStage
{
    public BindingDescriptionBuilder(IEnumerable<Type> serviceTypes) :
        base(serviceTypes)
    {
    }

    public IBindingStage IfNone => MarkAsFallbackAndReturn(this);

    public IOwnershipStage UseConstant(object instance)
    {
        return UseProvider(new ConstantInstanceProvider<object>(instance));
    }
}