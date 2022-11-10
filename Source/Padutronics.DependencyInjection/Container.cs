using Padutronics.DependencyInjection.Resolution.Activation;
using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection;

internal sealed class Container : IContainer
{
    private readonly IStorage storage;
    private readonly IEnumerable<IValueProvider> valueProviders;

    public Container(IStorage storage, IEnumerable<IValueProvider> valueProviders)
    {
        this.storage = storage;
        this.valueProviders = valueProviders;
    }

    public bool CanResolve(Type serviceType)
    {
        var canResolve = false;

        if (storage.TryGetBinding(serviceType, out Binding? binding))
        {
            ActivationSession session = CreateActivationSession();

            canResolve = binding.Activator.CanGetInstance(session);
        }

        return canResolve;
    }

    public bool CanResolve<TService>()
        where TService : class
    {
        return CanResolve(typeof(TService));
    }

    private ActivationSession CreateActivationSession()
    {
        return new ActivationSession(container: this, valueProviders);
    }

    public object Resolve(Type serviceType)
    {
        if (storage.TryGetBinding(serviceType, out Binding? binding))
        {
            ActivationSession session = CreateActivationSession();

            return binding.Activator.GetInstance(session);
        }

        throw new InvalidOperationException($"Service of type {serviceType} is not registered.");
    }

    public TService Resolve<TService>()
        where TService : class
    {
        return (TService)Resolve(typeof(TService));
    }
}