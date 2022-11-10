using Padutronics.DependencyInjection.Resolution.Activation;
using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using Padutronics.Disposing;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection;

internal sealed class Container : DisposableObject, IContainer
{
    private readonly IScope scope;
    private readonly IStorage storage;
    private readonly IEnumerable<IValueProvider> valueProviders;

    public Container(IStorage storage, IScope scope, IEnumerable<IValueProvider> valueProviders)
    {
        this.scope = scope;
        this.storage = storage;
        this.valueProviders = valueProviders;

        AddAdditionalBindings(storage);
    }

    private void AddAdditionalBindings(IStorage storage)
    {
        storage.AddConstantBinding<IContainer>(this);
        storage.AddConstantBinding<IContainerContext>(this);
    }

    public bool CanResolve(Type serviceType)
    {
        var canResolve = false;

        if (storage.TryGetBinding(serviceType, out Binding? binding))
        {
            ActivationSession session = CreateActivationSession(serviceType);

            canResolve = binding.ProfileProvider.DefaultProfile.Activator.CanGetInstance(session);
        }

        return canResolve;
    }

    public bool CanResolve<TService>()
        where TService : class
    {
        return CanResolve(typeof(TService));
    }

    private ActivationSession CreateActivationSession(Type serviceType)
    {
        return new ActivationSession(serviceType, containerContext: this, scope, valueProviders);
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            scope.Dispose();
        }
    }

    public object Resolve(Type serviceType)
    {
        if (storage.TryGetBinding(serviceType, out Binding? binding))
        {
            ActivationSession session = CreateActivationSession(serviceType);

            return binding.ProfileProvider.DefaultProfile.Activator.GetInstance(session);
        }

        throw new InvalidOperationException($"Service of type {serviceType} is not registered.");
    }

    public TService Resolve<TService>()
        where TService : class
    {
        return (TService)Resolve(typeof(TService));
    }
}