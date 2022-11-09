using System;

namespace Padutronics.DependencyInjection.Registration.Fluent;

public interface IBindingStage
{
    void Use(Type implementationType);
}