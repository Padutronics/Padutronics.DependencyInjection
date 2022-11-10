using Padutronics.DependencyInjection.Registration;
using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using System;
using System.Collections.Generic;

namespace Padutronics.DependencyInjection;

public sealed class ContainerBuilder : IContainerBuilder
{
    private readonly IBuildPlan buildPlan = new BuildPlan();

    private TBindingDescriptionBuilder AddBindingDescriptionBuilderToBuildPlan<TBindingDescriptionBuilder>(TBindingDescriptionBuilder bindingDescriptionBuilder)
        where TBindingDescriptionBuilder : IBindingDescriptionBuilder
    {
        buildPlan.Add(bindingDescriptionBuilder);

        return bindingDescriptionBuilder;
    }

    public IContainer Build()
    {
        var valueProviders = new IValueProvider[]
        {
            new AutowiringValueProvider(),
            /// <see cref="DefaultValueProvider" /> must be placed last to supply default value only if all other providers have failed to supply a value.
            new DefaultValueProvider()
        };

        IStorage storage = BuildStorage();

        return new Container(storage, valueProviders);
    }

    private IStorage BuildStorage()
    {
        IEnumerable<Binding> bindings = buildPlan.Build();

        return new Storage(bindings);
    }

    public IBindingStage For(Type serviceType)
    {
        return AddBindingDescriptionBuilderToBuildPlan(new BindingDescriptionBuilder(serviceType));
    }

    public IBindingStage<TService> For<TService>()
        where TService : class
    {
        return AddBindingDescriptionBuilderToBuildPlan(new BindingDescriptionBuilder<TService>());
    }
}