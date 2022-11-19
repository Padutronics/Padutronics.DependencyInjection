using Padutronics.DependencyInjection.Registration;
using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.DependencyInjection.Storages;
using Padutronics.Disposing.Disposers;
using Padutronics.Reflection.Types;
using System;
using System.Collections.Generic;
using System.Linq;

using Trace = Padutronics.Diagnostics.Tracing.Trace<Padutronics.DependencyInjection.ContainerBuilder>;

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
        Trace.CallStart("Started container building.");

        IStorage storage = BuildStorage();

        var valueProviders = new IValueProvider[]
        {
            new AutowiringValueProvider(),
            /// <see cref="DefaultValueProvider" /> must be placed last to supply default value only if all other providers have failed to supply a value.
            new DefaultValueProvider()
        };
        var scope = new Scope(new StackDisposer());

        var container = new Container(storage, scope, valueProviders);

        Trace.CallEnd("Finished container building.");

        return container;
    }

    private IStorage BuildStorage()
    {
        Trace.CallStart("Started storage building.");

        IEnumerable<Binding> bindings = buildPlan.Build();

        var bindingBuilders = new IBindingBuilder[]
        {
            new EnumerableBindingBuilder(),
            new LazyBindingBuilder()
        };

        var storage = new Storage(bindings, bindingBuilders);

        Trace.CallEnd("Finished storage building.");

        return storage;
    }

    public IFallbackBindingStage For(params Type[] serviceTypes)
    {
        return For((IEnumerable<Type>)serviceTypes);
    }

    public IFallbackBindingStage For(IEnumerable<Type> serviceTypes)
    {
        Trace.Call($"Started registration of services: {string.Join(", ", serviceTypes.Select(serviceType => serviceType))}.");

        return AddBindingDescriptionBuilderToBuildPlan(new BindingDescriptionBuilder(serviceTypes));
    }

    public IFallbackBindingStage<TService> For<TService>()
        where TService : class
    {
        Trace.Call($"Started registration of service: {typeof(TService)}.");

        return AddBindingDescriptionBuilderToBuildPlan(new BindingDescriptionBuilder<TService>());
    }

    public IFallbackBindingStage<TService1, TService2> For<TService1, TService2>()
        where TService1 : class
        where TService2 : class
    {
        Trace.Call($"Started registration of services: {string.Join(", ", TypeArray.Create<TService1, TService2>().Select(serviceType => serviceType))}.");

        return AddBindingDescriptionBuilderToBuildPlan(new BindingDescriptionBuilder<TService1, TService2>());
    }

    public IFallbackBindingStage<TService1, TService2, TService3> For<TService1, TService2, TService3>()
        where TService1 : class
        where TService2 : class
        where TService3 : class
    {
        Trace.Call($"Started registration of services: {string.Join(", ", TypeArray.Create<TService1, TService2, TService3>().Select(serviceType => serviceType))}.");

        return AddBindingDescriptionBuilderToBuildPlan(new BindingDescriptionBuilder<TService1, TService2, TService3>());
    }

    public IContainerBuilder IncludeModule(IContainerModule module)
    {
        Trace.CallStart();
        Trace.Information($"Including module: {module.GetType()}.");

        module.Load(containerBuilder: this);

        Trace.CallEnd();

        return this;
    }

    public IContainerBuilder IncludeModule<TModule>()
        where TModule : IContainerModule, new()
    {
        return IncludeModule(new TModule());
    }
}