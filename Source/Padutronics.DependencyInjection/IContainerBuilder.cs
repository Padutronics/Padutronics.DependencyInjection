using Padutronics.DependencyInjection.Registration.Fluent;
using System;

namespace Padutronics.DependencyInjection;

public interface IContainerBuilder
{
    IBindingStage For(Type serviceType);
    IBindingStage<TService> For<TService>()
        where TService : class;
}