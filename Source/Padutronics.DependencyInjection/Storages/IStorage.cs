using System;
using System.Diagnostics.CodeAnalysis;

namespace Padutronics.DependencyInjection.Storages;

internal interface IStorage
{
    bool TryGetBinding(Type serviceType, [NotNullWhen(true)] out Binding? binding);
}