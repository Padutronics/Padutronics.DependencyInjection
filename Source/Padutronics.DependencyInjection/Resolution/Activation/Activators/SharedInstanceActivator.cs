using System;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class SharedInstanceActivator : IActivator
{
    private readonly IActivator activator;

    public SharedInstanceActivator(IActivator activator)
    {
        this.activator = activator;
    }

    public bool CanGetInstance(ActivationSession session)
    {
        Type instanceType = GetInstanceType(session);

        return session.Scope.ContainsInstance(instanceType) || activator.CanGetInstance(session);
    }

    public object GetInstance(ActivationSession session)
    {
        Type instanceType = GetInstanceType(session);

        if (!session.Scope.TryGetInstance(instanceType, out object? instance))
        {
            instance = activator.GetInstance(session);

            session.Scope.AddInstance(instanceType, instance);
        }

        return instance;
    }

    public Type GetInstanceType(ActivationSession session)
    {
        return activator.GetInstanceType(session);
    }
}