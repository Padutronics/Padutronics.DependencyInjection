using Padutronics.DependencyInjection.Storages;
using System;

namespace Padutronics.DependencyInjection;

internal sealed class Container : IContainer
{
    private readonly IStorage storage;

    public Container(IStorage storage)
    {
        this.storage = storage;
    }

    public object Resolve(Type serviceType)
    {
        if (storage.TryGetBinding(serviceType, out Binding? binding))
        {
            return binding.Activator.GetInstance();
        }

        throw new InvalidOperationException($"Service of type {serviceType} is not registered.");
    }

    public TService Resolve<TService>()
        where TService : class
    {
        return (TService)Resolve(typeof(TService));
    }
}