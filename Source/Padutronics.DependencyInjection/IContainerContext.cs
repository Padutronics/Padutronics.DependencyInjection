using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection;

public interface IContainerContext
{
    bool CanResolve(Type serviceType, params IValueProvider[] valueProviders);
    bool CanResolve(Type serviceType, IEnumerable<IValueProvider> valueProviders);
    bool CanResolve<TService>(params IValueProvider[] valueProviders)
        where TService : class;
    bool CanResolve<TService>(IEnumerable<IValueProvider> valueProviders)
        where TService : class;
    object Resolve(Type serviceType, params IValueProvider[] valueProviders);
    object Resolve(Type serviceType, IEnumerable<IValueProvider> valueProviders);
    TService Resolve<TService>(params IValueProvider[] valueProviders)
        where TService : class;
    TService Resolve<TService>(IEnumerable<IValueProvider> valueProviders)
        where TService : class;
    IEnumerable<object> ResolveAll(Type serviceType, params IValueProvider[] valueProviders);
    IEnumerable<object> ResolveAll(Type serviceType, IEnumerable<IValueProvider> valueProviders);
    IEnumerable<TService> ResolveAll<TService>(params IValueProvider[] valueProviders)
        where TService : class;
    IEnumerable<TService> ResolveAll<TService>(IEnumerable<IValueProvider> valueProviders)
        where TService : class;
}