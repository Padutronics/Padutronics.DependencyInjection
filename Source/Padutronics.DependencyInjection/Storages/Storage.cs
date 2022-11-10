using Padutronics.DependencyInjection.Resolution;
using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class Storage : IStorage
{
    private readonly IDictionary<Type, Binding> serviceTypeToBindingMappings = new Dictionary<Type, Binding>();

    public Storage(IEnumerable<Binding> bindings)
    {
        serviceTypeToBindingMappings = bindings.ToDictionary(
            binding => binding.ServiceType,
            binding => binding
        );
    }

    public void AddBinding(Type serviceType, Binding binding)
    {
        serviceTypeToBindingMappings.Add(serviceType, binding);
    }

    public void AddConstantBinding<TImplementation>(TImplementation instance)
        where TImplementation : class
    {
        Type serviceType = typeof(TImplementation);

        var binding = new Binding(
            serviceType,
            new DefaultProfileProvider(
                new Profile(
                    new InstanceProviderActivator<TImplementation>(
                        new ConstantInstanceProvider<TImplementation>(instance)
                    ),
                    isFallback: false
                )
            )
        );

        AddBinding(serviceType, binding);
    }

    public bool TryGetBinding(Type serviceType, [NotNullWhen(true)] out Binding? binding)
    {
        var isBindingFound = true;

        if (!serviceTypeToBindingMappings.TryGetValue(serviceType, out binding))
        {
            if (serviceType.IsGenericType && !serviceType.IsGenericTypeDefinition)
            {
                Type openGenericServiceType = serviceType.GetGenericTypeDefinition();

                isBindingFound = TryGetBinding(openGenericServiceType, out binding);
            }
            else
            {
                isBindingFound = false;
            }
        }

        return isBindingFound;
    }
}