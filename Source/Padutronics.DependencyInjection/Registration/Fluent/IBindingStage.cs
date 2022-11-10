using System;

namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IBindingStage
{
    ILifetimeStage Use(Type implementationType);
    IOwnershipStage UseConstant(object instance);
}