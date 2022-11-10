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
            .SelectMany(
                bindingDescription => bindingDescription.ServiceTypes.Select(
                    serviceType => new
                    {
                        Profile = new Profile(bindingDescription.Activator, bindingDescription.IsFallback),
                        ServiceType = serviceType
                    }
                )
            )
            .GroupBy(bindingData => bindingData.ServiceType)
            .ToDictionary(
                serviceTypeToBindingDataMapping => serviceTypeToBindingDataMapping.Key,
                serviceTypeToBindingDataMapping => serviceTypeToBindingDataMapping.Select(bindingData => bindingData.Profile)
            )
            .Select(
                serviceTypeToProfilesMapping => new Binding(
                    serviceTypeToProfilesMapping.Key,
                    new AllProfileProvider(serviceTypeToProfilesMapping.Value)
                )
            )
            .ToList();
    }
}