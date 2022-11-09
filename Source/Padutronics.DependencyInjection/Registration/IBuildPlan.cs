using Padutronics.DependencyInjection.Storages;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Registration;

internal interface IBuildPlan
{
    void Add(IBindingDescriptionBuilder bindingDescriptionBuilder);
    IEnumerable<Binding> Build();
}