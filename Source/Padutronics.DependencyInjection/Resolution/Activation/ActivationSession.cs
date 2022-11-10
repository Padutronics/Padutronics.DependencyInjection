using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Resolution.Activation;

internal sealed class ActivationSession
{
    public ActivationSession(Type serviceType, IContainerContext containerContext, IScope scope, IEnumerable<IValueProvider> valueProviders)
    {
        ContainerContext = containerContext;
        Scope = scope;
        ServiceType = serviceType;
        ValueProviders = valueProviders;
    }

    public IContainerContext ContainerContext { get; }

    public IScope Scope { get; }

    public Type ServiceType { get; }

    public IEnumerable<IValueProvider> ValueProviders { get; }
}