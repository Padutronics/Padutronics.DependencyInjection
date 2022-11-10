using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescription
{
    public BindingDescription(IEnumerable<Type> serviceTypes, IActivator activator, bool isFallback)
    {
        Activator = activator;
        IsFallback = isFallback;
        ServiceTypes = serviceTypes;
    }

    public IActivator Activator { get; }

    public bool IsFallback { get; }

    public IEnumerable<Type> ServiceTypes { get; }
}