using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using System;
using System.Linq;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class LazyBindingBuilder : IBindingBuilder
{
    public Binding Build(Type serviceType)
    {
        Type valueType = serviceType
            .GetGenericArguments()
            .First();

        return new Binding(
            serviceType,
            new DefaultProfileProvider(
                new Profile(
                    new LazyActivator(valueType),
                    isFallback: false
                )
            )
        );
    }

    public bool CanBuild(Type serviceType)
    {
        return serviceType.IsGenericType && !serviceType.IsGenericTypeDefinition && serviceType.GetGenericTypeDefinition() == typeof(Lazy<>);
    }
}