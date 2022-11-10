using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class Scope : IScope
{
    private readonly IDictionary<Type, object> typeToInstanceMappings = new Dictionary<Type, object>();

    public void AddInstance(Type type, object instance)
    {
        typeToInstanceMappings.Add(type, instance);
    }

    public bool ContainsInstance(Type type)
    {
        return typeToInstanceMappings.ContainsKey(type);
    }

    public bool TryGetInstance(Type type, [NotNullWhen(true)] out object? instance)
    {
        return typeToInstanceMappings.TryGetValue(type, out instance);
    }
}