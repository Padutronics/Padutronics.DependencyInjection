using System;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class DisposableActivator : IActivator
{
    private readonly IActivator activator;

    private bool isInstanceAddedForDisposal;

    public DisposableActivator(IActivator activator)
    {
        this.activator = activator;
    }

    public bool CanGetInstance(ActivationSession session)
    {
        return activator.CanGetInstance(session);
    }

    public object GetInstance(ActivationSession session)
    {
        object instance = activator.GetInstance(session);

        if (!isInstanceAddedForDisposal && instance is IDisposable disposableInstance)
        {
            session.Scope.Disposer.AddInstanceForDisposal(disposableInstance);

            isInstanceAddedForDisposal = true;
        }

        return instance;
    }

    public Type GetInstanceType(ActivationSession session)
    {
        return activator.GetInstanceType(session);
    }
}