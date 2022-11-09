using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class Storage : IStorage
{
    private readonly IReadOnlyDictionary<Type, Binding> serviceTypeToBindingMappings = new Dictionary<Type, Binding>();

    public Storage(IEnumerable<Binding> bindings)
    {
        serviceTypeToBindingMappings = bindings.ToDictionary(
            binding => binding.ServiceType,
            binding => binding
        );
    }

    public bool TryGetBinding(Type serviceType, [NotNullWhen(true)] out Binding? binding)
    {
        return serviceTypeToBindingMappings.TryGetValue(serviceType, out binding);
    }
}