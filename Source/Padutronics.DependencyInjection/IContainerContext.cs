using System;

namespace Padutronics.DependencyInjection;

public interface IContainerContext
{
    bool CanResolve(Type serviceType);
    bool CanResolve<TService>()
        where TService : class;
    object Resolve(Type serviceType);
    TService Resolve<TService>()
        where TService : class;
}