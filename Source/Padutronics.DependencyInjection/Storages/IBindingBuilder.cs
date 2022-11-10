using System;

namespace Padutronics.DependencyInjection.Storages;

internal interface IBindingBuilder
{
    Binding Build(Type serviceType);
    bool CanBuild(Type serviceType);
}