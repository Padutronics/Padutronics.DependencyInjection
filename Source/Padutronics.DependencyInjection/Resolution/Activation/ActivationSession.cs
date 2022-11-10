using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Resolution.Activation;

internal sealed class ActivationSession
{
    public ActivationSession(IContainerContext containerContext, IScope scope, IEnumerable<IValueProvider> valueProviders)
    {
        ContainerContext = containerContext;
        Scope = scope;
        ValueProviders = valueProviders;
    }

    public IContainerContext ContainerContext { get; }

    public IScope Scope { get; }

    public IEnumerable<IValueProvider> ValueProviders { get; }
}