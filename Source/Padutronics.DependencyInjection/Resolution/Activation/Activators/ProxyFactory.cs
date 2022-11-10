using System;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class ProxyFactory
{
    private readonly Func<IContainerContext, object> instanceFactory;

    public ProxyFactory(Type type, Func<IContainerContext, object> instanceFactory)
    {
        this.instanceFactory = instanceFactory;
        Type = type;
    }

    public Type Type { get; }

    public object CreateInstance(IContainerContext containerContext)
    {
        return instanceFactory(containerContext);
    }
}