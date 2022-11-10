using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Resolution.Activation;

internal sealed class ActivationSession
{
    public ActivationSession(IContainer container, IScope scope, IEnumerable<IValueProvider> valueProviders)
    {
        Container = container;
        Scope = scope;
        ValueProviders = valueProviders;
    }

    public IContainer Container { get; }

    public IScope Scope { get; }

    public IEnumerable<IValueProvider> ValueProviders { get; }
}