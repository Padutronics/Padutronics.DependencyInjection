using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution;
using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using Padutronics.Reflection.Constructors.Finders;
using Padutronics.Reflection.Constructors.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Padutronics.DependencyInjection.Registration;

internal abstract class BindingDescriptionBuilderBase : IBindingDescriptionBuilder, ILifetimeStage, IOwnershipStage
{
    private readonly IEnumerable<Type> serviceTypes;

    private IActivator? activator;
    private bool isFallback;
    private bool isOwnedByContainer = true;

    protected BindingDescriptionBuilderBase(IEnumerable<Type> serviceTypes)
    {
        this.serviceTypes = serviceTypes;
    }

    public BindingDescription Build()
    {
        if (activator is null)
        {
            throw new InvalidOperationException($"Registration for services of types {{ {string.Join(", ", serviceTypes)} }} was not completed.");
        }

        if (isOwnedByContainer)
        {
            activator = new DisposableActivator(activator);
        }

        return new BindingDescription(serviceTypes, activator, isFallback);
    }

    public void ExternallyOwned()
    {
        isOwnedByContainer = false;
    }

    public void InstancePerDependency()
    {
        // Do nothing.
    }

    protected T MarkAsFallbackAndReturn<T>(T value)
    {
        isFallback = true;

        return value;
    }

    public void OwnedByContainer()
    {
        // Do nothing.
    }

    public IOwnershipStage SingleInstance()
    {
        if (activator is null)
        {
            throw new InvalidOperationException($"Activator for services of types {{ {string.Join(", ", serviceTypes)} }} was not configured.");
        }

        activator = new SharedInstanceActivator(activator);

        return this;
    }

    public ILifetimeStage Use(Type implementationType)
    {
        activator = new TypeActivator(implementationType, new PublicConstructorFinder(), new LongestConstructorSelector());

        return this;
    }

    protected IOwnershipStage UseProvider<TImplementation>(IInstanceProvider<TImplementation> instanceProvider)
        where TImplementation : class
    {
        activator = new InstanceProviderActivator<TImplementation>(instanceProvider);

        return this;
    }

    public ILifetimeStage UseSelf()
    {
        return Use(serviceTypes.Single());
    }
}