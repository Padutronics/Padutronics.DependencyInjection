using System;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class InstanceProviderActivator<TImplementation> : IActivator
    where TImplementation : class
{
    private readonly IInstanceProvider<TImplementation> instanceProvider;

    public InstanceProviderActivator(IInstanceProvider<TImplementation> instanceProvider)
    {
        this.instanceProvider = instanceProvider;
    }

    public bool CanGetInstance(ActivationSession session)
    {
        return true;
    }

    public object GetInstance(ActivationSession session)
    {
        return instanceProvider.GetInstance(session);
    }

    public Type GetInstanceType(ActivationSession session)
    {
        return typeof(TImplementation);
    }
}