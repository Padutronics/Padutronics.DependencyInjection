using System;
using System.Collections.Generic;
using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class EnumerableActivator : IActivator
{
    private readonly Type itemType;
    private readonly Lazy<MethodInfo> resolveMethod;

    public EnumerableActivator(Type itemType)
    {
        if (itemType.IsValueType)
        {
            throw new ArgumentException($"Type {itemType} is not a reference type.", nameof(itemType));
        }

        this.itemType = itemType;

        resolveMethod = new Lazy<MethodInfo>(GetResolveMethod);
    }

    public bool CanGetInstance(ActivationSession session)
    {
        return true;
    }

    public object GetInstance(ActivationSession session)
    {
        return resolveMethod.Value.Invoke(obj: this, parameters: new object[] { session }) ?? throw new InvalidOperationException("Resolve method returned null.");
    }

    public Type GetInstanceType(ActivationSession session)
    {
        return typeof(IEnumerable<>).MakeGenericType(itemType);
    }

    private MethodInfo GetResolveMethod()
    {
        MethodInfo? resolveMethod = typeof(EnumerableActivator).GetMethod(nameof(Resolve), BindingFlags.Instance | BindingFlags.NonPublic);
        if (resolveMethod is null)
        {
            throw new InvalidOperationException("Resolve method is not found.");
        }

        return resolveMethod
            .GetGenericMethodDefinition()
            .MakeGenericMethod(itemType);
    }

    private IEnumerable<TItem> Resolve<TItem>(ActivationSession session)
        where TItem : class
    {
        return session.ContainerContext.ResolveAll<TItem>(session.AdditionalValueProviders);
    }
}