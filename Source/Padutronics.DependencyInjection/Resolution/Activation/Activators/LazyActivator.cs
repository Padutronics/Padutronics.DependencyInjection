using System;
using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class LazyActivator : IActivator
{
    private readonly Lazy<MethodInfo> resolveMethod;
    private readonly Type valueType;

    public LazyActivator(Type valueType)
    {
        if (valueType.IsValueType)
        {
            throw new ArgumentException($"Type {valueType} is not a reference type.", nameof(valueType));
        }

        this.valueType = valueType;

        resolveMethod = new Lazy<MethodInfo>(GetResolveMethod);
    }

    public bool CanGetInstance(ActivationSession session)
    {
        return session.ContainerContext.CanResolve(valueType);
    }

    public object GetInstance(ActivationSession session)
    {
        return resolveMethod.Value.Invoke(obj: this, parameters: new[] { session }) ?? throw new InvalidOperationException("Resolve method returned null.");
    }

    public Type GetInstanceType(ActivationSession session)
    {
        return typeof(Lazy<>).MakeGenericType(valueType);
    }

    private MethodInfo GetResolveMethod()
    {
        MethodInfo? resolveMethod = typeof(LazyActivator).GetMethod(nameof(Resolve), BindingFlags.Instance | BindingFlags.NonPublic);
        if (resolveMethod is null)
        {
            throw new InvalidOperationException("Resolve method is not found.");
        }

        return resolveMethod
            .GetGenericMethodDefinition()
            .MakeGenericMethod(valueType);
    }

    private Lazy<TValue> Resolve<TValue>(ActivationSession session)
        where TValue : class
    {
        return new Lazy<TValue>(() => session.ContainerContext.Resolve<TValue>());
    }
}