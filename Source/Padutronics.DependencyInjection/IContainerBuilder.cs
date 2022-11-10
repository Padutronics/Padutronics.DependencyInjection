using Padutronics.DependencyInjection.Registration.Fluent;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection;

public interface IContainerBuilder
{
    IFallbackBindingStage For(params Type[] serviceTypes);
    IFallbackBindingStage For(IEnumerable<Type> serviceTypes);
    IFallbackBindingStage<TService> For<TService>()
        where TService : class;
    IFallbackBindingStage<TService1, TService2> For<TService1, TService2>()
        where TService1 : class
        where TService2 : class;
    IFallbackBindingStage<TService1, TService2, TService3> For<TService1, TService2, TService3>()
        where TService1 : class
        where TService2 : class
        where TService3 : class;
}