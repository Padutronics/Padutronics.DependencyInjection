using System;

namespace Padutronics.DependencyInjection;

public interface IContainer
{
    object Resolve(Type serviceType);
    TService Resolve<TService>()
        where TService : class;
}