using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using System;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class Binding
{
    public Binding(Type serviceType, IActivator activator)
    {
        Activator = activator;
        ServiceType = serviceType;
    }

    public IActivator Activator { get; }

    public Type ServiceType { get; }
}