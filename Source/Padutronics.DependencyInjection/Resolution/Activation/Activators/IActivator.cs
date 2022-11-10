namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal interface IActivator
{
    bool CanGetInstance(ActivationSession session);
    object GetInstance(ActivationSession session);
}