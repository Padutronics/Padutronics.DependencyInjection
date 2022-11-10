using System;
using System.Diagnostics.CodeAnalysis;

namespace Padutronics.DependencyInjection.Storages;

internal interface IScope
{
    void AddInstance(Type type, object instance);
    bool ContainsInstance(Type type);
    bool TryGetInstance(Type type, [NotNullWhen(true)] out object? instance);
}