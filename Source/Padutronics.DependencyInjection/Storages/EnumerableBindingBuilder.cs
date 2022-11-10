using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class EnumerableBindingBuilder : IBindingBuilder
{
    public Binding Build(Type serviceType)
    {
        Type itemType = serviceType
            .GetGenericArguments()
            .First();

        return new Binding(
            serviceType,
            new DefaultProfileProvider(
                new Profile(
                    new EnumerableActivator(itemType),
                    isFallback: false
                )
            )
        );
    }

    public bool CanBuild(Type serviceType)
    {
        return serviceType.IsGenericType && !serviceType.IsGenericTypeDefinition && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }
}