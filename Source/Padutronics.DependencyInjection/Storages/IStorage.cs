using System;
using System.Diagnostics.CodeAnalysis;

namespace Padutronics.DependencyInjection.Storages;

internal interface IStorage
{
    void AddBinding(Type serviceType, Binding binding);
    void AddConstantBinding<TImplementation>(TImplementation instance)
        where TImplementation : class;
    bool TryGetBinding(Type serviceType, [NotNullWhen(true)] out Binding? binding);
}