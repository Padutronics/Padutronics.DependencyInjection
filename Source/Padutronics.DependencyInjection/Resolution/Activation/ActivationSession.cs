using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Padutronics.DependencyInjection.Resolution.Activation;

internal sealed class ActivationSession
{
    public ActivationSession(Type serviceType, IContainerContext containerContext, IScope scope, IEnumerable<IValueProvider> defaultValueProviders, IEnumerable<IValueProvider> additionalValueProviders)
    {
        AdditionalValueProviders = additionalValueProviders;
        ContainerContext = containerContext;
        DefaultValueProviders = defaultValueProviders;
        Scope = scope;
        ServiceType = serviceType;
    }

    public IEnumerable<IValueProvider> AdditionalValueProviders { get; }

    // Additional value providers must be placed before default ones to get higher priority.
    public IEnumerable<IValueProvider> AllValueProviders => AdditionalValueProviders.Concat(DefaultValueProviders);

    public IContainerContext ContainerContext { get; }

    public IEnumerable<IValueProvider> DefaultValueProviders { get; }

    public IScope Scope { get; }

    public Type ServiceType { get; }
}