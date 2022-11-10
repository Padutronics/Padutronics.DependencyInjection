using Padutronics.DependencyInjection.Resolution.Activation.Activators;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class Profile
{
    public Profile(IActivator activator, bool isFallback)
    {
        Activator = activator;
        IsFallback = isFallback;
    }

    public IActivator Activator { get; }

    public bool IsFallback { get; }
}