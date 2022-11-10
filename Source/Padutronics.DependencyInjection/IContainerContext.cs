using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection;

public interface IContainerContext
{
    bool CanResolve(Type serviceType);
    bool CanResolve<TService>()
        where TService : class;
    object Resolve(Type serviceType);
    TService Resolve<TService>()
        where TService : class;
    IEnumerable<object> ResolveAll(Type serviceType);
    IEnumerable<TService> ResolveAll<TService>()
        where TService : class;
}