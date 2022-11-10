using Padutronics.DependencyInjection.Registration.Fluent;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection;

public interface IContainerBuilder
{
    IBindingStage For(params Type[] serviceTypes);
    IBindingStage For(IEnumerable<Type> serviceTypes);
    IBindingStage<TService> For<TService>()
        where TService : class;
    IBindingStage<TService1, TService2> For<TService1, TService2>()
        where TService1 : class
        where TService2 : class;
    IBindingStage<TService1, TService2, TService3> For<TService1, TService2, TService3>()
        where TService1 : class
        where TService2 : class
        where TService3 : class;
}