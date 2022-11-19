using Padutronics.DependencyInjection.Storages;
using System.Collections.Generic;
using System.Linq;

using Trace = Padutronics.Diagnostics.Tracing.Trace<Padutronics.DependencyInjection.Registration.BuildPlan>;

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
        Trace.CallStart("Started build plan.");

        IEnumerable<BindingDescription> bindingDescriptions = bindingDescriptionBuilders
            .Select(bindingDescriptionBuilder => bindingDescriptionBuilder.Build())
            .ToList();

        Trace.Information($"Found {bindingDescriptions.Count()} binding descriptions.");

        IEnumerable<Binding> bindings = bindingDescriptions
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

        Trace.Information($"Created {bindings.Count()} bindings.");
        Trace.CallEnd("Finished build plan.");

        return bindings;
    }
}