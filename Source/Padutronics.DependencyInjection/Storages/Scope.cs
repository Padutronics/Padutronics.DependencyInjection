using Padutronics.Disposing;
using Padutronics.Disposing.Disposers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class Scope : DisposableObject, IScope
{
    private readonly IDictionary<Type, object> typeToInstanceMappings = new Dictionary<Type, object>();

    public Scope(IDisposer disposer)
    {
        Disposer = disposer;
    }

    public IDisposer Disposer { get; }

    public void AddInstance(Type type, object instance)
    {
        typeToInstanceMappings.Add(type, instance);
    }

    public bool ContainsInstance(Type type)
    {
        return typeToInstanceMappings.ContainsKey(type);
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            Disposer.Dispose();
        }
    }

    public bool TryGetInstance(Type type, [NotNullWhen(true)] out object? instance)
    {
        return typeToInstanceMappings.TryGetValue(type, out instance);
    }
}