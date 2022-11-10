using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescription
{
    public BindingDescription(IEnumerable<Type> serviceTypes, IActivator activator)
    {
        Activator = activator;
        ServiceTypes = serviceTypes;
    }

    public IActivator Activator { get; }

    public IEnumerable<Type> ServiceTypes { get; }
}