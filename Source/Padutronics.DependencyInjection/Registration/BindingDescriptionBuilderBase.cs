using Padutronics.DependencyInjection.Registration.Fluent;
using Padutronics.DependencyInjection.Resolution;
using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using Padutronics.Reflection.Constructors.Finders;
using Padutronics.Reflection.Constructors.Selectors;
using System;

namespace Padutronics.DependencyInjection.Registration;

internal abstract class BindingDescriptionBuilderBase : IBindingDescriptionBuilder, ILifetimeStage
{
    private readonly Type serviceType;

    private IActivator? activator;

    protected BindingDescriptionBuilderBase(Type serviceType)
    {
        this.serviceType = serviceType;
    }

    public BindingDescription Build()
    {
        if (activator is null)
        {
            throw new InvalidOperationException($"Registration for service of type {serviceType} was not completed.");
        }

        return new BindingDescription(serviceType, activator);
    }

    public void InstancePerDependency()
    {
        // Do nothing.
    }

    public void SingleInstance()
    {
        if (activator is null)
        {
            throw new InvalidOperationException($"Activator for service of type {serviceType} was not configured.");
        }

        activator = new SharedInstanceActivator(activator);
    }

    public ILifetimeStage Use(Type implementationType)
    {
        activator = new TypeActivator(implementationType, new PublicConstructorFinder(), new LongestConstructorSelector());

        return this;
    }

    protected void UseProvider<TImplementation>(IInstanceProvider<TImplementation> instanceProvider)
        where TImplementation : class
    {
        activator = new InstanceProviderActivator<TImplementation>(instanceProvider);
    }
}