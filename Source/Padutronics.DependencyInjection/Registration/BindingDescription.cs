using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using System;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BindingDescription
{
    public BindingDescription(Type serviceType, IActivator activator)
    {
        Activator = activator;
        ServiceType = serviceType;
    }

    public IActivator Activator { get; }

    public Type ServiceType { get; }
}