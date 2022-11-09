using System;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class TypeActivator : IActivator
{
    private readonly Type type;

    public TypeActivator(Type type)
    {
        this.type = type;
    }

    public object GetInstance()
    {
        try
        {
            return Activator.CreateInstance(type) ?? throw new InvalidOperationException("Activator returned null.");
        }
        catch (MissingMethodException)
        {
            throw new InvalidOperationException($"No default public constructor on type {type} can be found.");
        }
    }
}