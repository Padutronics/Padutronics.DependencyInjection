using Padutronics.DependencyInjection.Resolution.Activation;
using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using Padutronics.Disposing;
using System;
using System.Collections.Generic;
using System.Linq;

using Trace = Padutronics.Diagnostics.Tracing.Trace<Padutronics.DependencyInjection.Container>;

namespace Padutronics.DependencyInjection;

internal sealed class Container : DisposableObject, IContainer
{
    private readonly IEnumerable<IValueProvider> defaultValueProviders;
    private readonly IScope scope;
    private readonly IStorage storage;

    public Container(IStorage storage, IScope scope, IEnumerable<IValueProvider> defaultValueProviders)
    {
        this.defaultValueProviders = defaultValueProviders;
        this.scope = scope;
        this.storage = storage;

        AddAdditionalBindings(storage);
    }

    private void AddAdditionalBindings(IStorage storage)
    {
        storage.AddConstantBinding<IContainer>(this);
        storage.AddConstantBinding<IContainerContext>(this);
    }

    public bool CanResolve(Type serviceType, params IValueProvider[] valueProviders)
    {
        return CanResolve(serviceType, (IEnumerable<IValueProvider>)valueProviders);
    }

    public bool CanResolve(Type serviceType, IEnumerable<IValueProvider> valueProviders)
    {
        var canResolve = false;

        if (storage.TryGetBinding(serviceType, out Binding? binding))
        {
            ActivationSession session = CreateActivationSession(serviceType, valueProviders);

            canResolve = binding.ProfileProvider.DefaultProfile.Activator.CanGetInstance(session);
        }

        return canResolve;
    }

    public bool CanResolve<TService>(params IValueProvider[] valueProviders)
        where TService : class
    {
        return CanResolve<TService>((IEnumerable<IValueProvider>)valueProviders);
    }

    public bool CanResolve<TService>(IEnumerable<IValueProvider> valueProviders)
        where TService : class
    {
        return CanResolve(typeof(TService), valueProviders);
    }

    private ActivationSession CreateActivationSession(Type serviceType, IEnumerable<IValueProvider> valueProviders)
    {
        return new ActivationSession(serviceType, containerContext: this, scope, defaultValueProviders, valueProviders);
    }

    protected override void Dispose(bool isDisposing)
    {
        Trace.CallStart("Started container disposing.");

        if (isDisposing)
        {
            scope.Dispose();
        }

        Trace.CallEnd("Finished container disposing.");
    }

    public object Resolve(Type serviceType, params IValueProvider[] valueProviders)
    {
        return Resolve(serviceType, (IEnumerable<IValueProvider>)valueProviders);
    }

    public object Resolve(Type serviceType, IEnumerable<IValueProvider> valueProviders)
    {
        try
        {
            Trace.CallStart($"Requested service {serviceType}.");

            if (storage.TryGetBinding(serviceType, out Binding? binding))
            {
                ActivationSession session = CreateActivationSession(serviceType, valueProviders);

                return binding.ProfileProvider.DefaultProfile.Activator.GetInstance(session);
            }

            Trace.Error($"Requested service {serviceType} is not resolved.");

            throw new InvalidOperationException($"Service of type {serviceType} is not registered.");
        }
        finally
        {
            Trace.CallEnd();
        }
    }

    public TService Resolve<TService>(params IValueProvider[] valueProviders)
        where TService : class
    {
        return Resolve<TService>((IEnumerable<IValueProvider>)valueProviders);
    }

    public TService Resolve<TService>(IEnumerable<IValueProvider> valueProviders)
        where TService : class
    {
        return (TService)Resolve(typeof(TService), valueProviders);
    }

    public IEnumerable<object> ResolveAll(Type serviceType, params IValueProvider[] valueProviders)
    {
        return ResolveAll(serviceType, (IEnumerable<IValueProvider>)valueProviders);
    }

    public IEnumerable<object> ResolveAll(Type serviceType, IEnumerable<IValueProvider> valueProviders)
    {
        try
        {
            Trace.CallStart($"Requested service {serviceType}.");

            IEnumerable<object> instances;

            if (storage.TryGetBinding(serviceType, out Binding? binding))
            {
                ActivationSession session = CreateActivationSession(serviceType, valueProviders);

                instances = binding.ProfileProvider.AllProfiles
                    .Select(profile => profile.Activator.GetInstance(session))
                    .ToList();
            }
            else
            {
                Trace.Warning($"Requested service {serviceType} is not resolved.");

                instances = Enumerable.Empty<object>();
            }

            return instances;
        }
        finally
        {
            Trace.CallEnd();
        }
    }

    public IEnumerable<TService> ResolveAll<TService>(params IValueProvider[] valueProviders)
        where TService : class
    {
        return ResolveAll<TService>((IEnumerable<IValueProvider>)valueProviders);
    }

    public IEnumerable<TService> ResolveAll<TService>(IEnumerable<IValueProvider> valueProviders)
        where TService : class
    {
        return ResolveAll(typeof(TService), valueProviders)
            .Cast<TService>()
            .ToList();
    }
}