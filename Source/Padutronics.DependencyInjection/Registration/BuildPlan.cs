using Padutronics.DependencyInjection.Storages;
using System.Collections.Generic;
using System.Linq;

namespace Padutronics.DependencyInjection.Registration;

internal sealed class BuildPlan : IBuildPlan
{
    private readonly ICollection<IBindingDescriptionBuilder> bindingDescriptionBuilders = new List<IBindingDescriptionBuilder>();

    public void Add(IBindingDescriptionBuilder bindingDescriptionBuilder)
    {
        bindingDescriptionBuilders.Add(bindingDescriptionBuilder);
    }

    public IEnumerable<Binding> Build()
    {
        return bindingDescriptionBuilders
            .Select(bindingDescriptionBuilder => bindingDescriptionBuilder.Build())
            .SelectMany(bindingDescription => bindingDescription.ServiceTypes.Select(serviceType => new Binding(serviceType, bindingDescription.Activator)))
            .ToList();
    }
}