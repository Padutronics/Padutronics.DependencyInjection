using Padutronics.DependencyInjection.Resolution.Activation.Activators;
using Padutronics.Reflection.Constructors.Finders;
using Padutronics.Reflection.Constructors.Selectors;
using System;

namespace Padutronics.DependencyInjection.Registration;

internal abstract class BindingDescriptionBuilderBase : IBindingDescriptionBuilder
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

    public void Use(Type implementationType)
    {
        activator = new TypeActivator(implementationType, new PublicConstructorFinder(), new LongestConstructorSelector());
    }
}