using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Resolution.Activation;

internal sealed class ActivationSession
{
    public ActivationSession(IContainer container, IEnumerable<IValueProvider> valueProviders)
    {
        Container = container;
        ValueProviders = valueProviders;
    }

    public IContainer Container { get; }

    public IEnumerable<IValueProvider> ValueProviders { get; }
}