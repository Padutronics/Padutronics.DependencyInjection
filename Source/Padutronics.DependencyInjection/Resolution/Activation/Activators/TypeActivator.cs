using Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;
using Padutronics.Reflection.Constructors.Finders;
using Padutronics.Reflection.Constructors.Selectors;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.Activators;

internal sealed class TypeActivator : IActivator
{
    private readonly IConstructorFinder constructorFinder;
    private readonly IConstructorSelector constructorSelector;
    private readonly Type type;

    public TypeActivator(Type type, IConstructorFinder constructorFinder, IConstructorSelector constructorSelector)
    {
        this.constructorFinder = constructorFinder;
        this.constructorSelector = constructorSelector;
        this.type = type;
    }

    public bool CanGetInstance(ActivationSession session)
    {
        var canGetInstance = false;

        Type instanceType = GetInstanceType(session);

        if (TrySelectConstructor(instanceType, session, out ConstructorInfo? selectedConstructor))
        {
            var canGetAllValues = true;

            ParameterInfo[] parameters = selectedConstructor.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                var canGetValue = false;

                foreach (IValueProvider valueProviders in session.ValueProviders)
                {
                    if (valueProviders.CanGetValue(parameter, session.ContainerContext))
                    {
                        canGetValue = true;
                        break;
                    }
                }

                if (!canGetValue)
                {
                    canGetAllValues = false;
                    break;
                }
            }

            canGetInstance = canGetAllValues;
        }

        return canGetInstance;
    }

    public object GetInstance(ActivationSession session)
    {
        Type instanceType = GetInstanceType(session);

        ConstructorInfo selectedConstructor = SelectConstructor(instanceType, session);

        object?[] parameters = GetParameters(selectedConstructor, session);

        return selectedConstructor.Invoke(parameters);
    }

    public Type GetInstanceType(ActivationSession session)
    {
        return type.IsGenericTypeDefinition
            ? type.MakeGenericType(session.ServiceType.GetGenericArguments())
            : type;
    }

    private IEnumerable<ConstructorInfo> GetInstantiableConstructors(IEnumerable<ConstructorInfo> constructors, ActivationSession session)
    {
        return constructors.Where(
            constructor => constructor
                .GetParameters()
                .All(
                    parameter => session.ValueProviders.Any(
                        valueProvider => valueProvider.CanGetValue(
                            parameter,
                            session.ContainerContext
                        )
                    )
                )
        );
    }

    private object?[] GetParameters(ConstructorInfo constructor, ActivationSession session)
    {
        var values = new List<object?>();

        ParameterInfo[] parameters = constructor.GetParameters();
        foreach (ParameterInfo parameter in parameters)
        {
            var canGetValue = false;

            foreach (IValueProvider valueProvider in session.ValueProviders)
            {
                if (valueProvider.CanGetValue(parameter, session.ContainerContext))
                {
                    object? value = valueProvider.GetValue(parameter, session.ContainerContext);

                    values.Add(value);

                    canGetValue = true;
                    break;
                }
            }

            if (!canGetValue)
            {
                throw new InvalidOperationException($"Service of type {parameter.ParameterType} is not registered.");
            }
        }

        return values.ToArray();
    }

    private ConstructorInfo SelectConstructor(Type targetType, ActivationSession session)
    {
        IEnumerable<ConstructorInfo> foundConstructors = constructorFinder.FindConstructors(targetType);
        if (!foundConstructors.Any())
        {
            throw new InvalidOperationException($"No constructors on type {targetType} can be found with the constructor finder {constructorFinder}.");
        }

        IEnumerable<ConstructorInfo> instantiableConstructors = GetInstantiableConstructors(foundConstructors, session);
        if (!instantiableConstructors.Any())
        {
            throw new InvalidOperationException($"Multiple constructors on type {targetType} found with constructor finder {constructorFinder}, but none can be instantiated.");
        }

        IEnumerable<ConstructorInfo> selectedConstructors = constructorSelector.SelectConstructors(instantiableConstructors);

        return selectedConstructors.Count() switch
        {
            0 => throw new InvalidOperationException($"No constructors on type {targetType} can be selected with the constructor selector {constructorSelector}."),
            > 1 => throw new InvalidOperationException($"Multiple constructors on type {targetType} can be selected with the constructor selector {constructorSelector}."),
            _ => selectedConstructors.Single()
        };
    }

    private bool TrySelectConstructor(Type targetType, ActivationSession session, [NotNullWhen(true)] out ConstructorInfo? selectedConstructor)
    {
        selectedConstructor = null;

        IEnumerable<ConstructorInfo> foundConstructors = constructorFinder.FindConstructors(targetType);
        if (foundConstructors.Any())
        {
            IEnumerable<ConstructorInfo> instantiableConstructors = GetInstantiableConstructors(foundConstructors, session);
            IEnumerable<ConstructorInfo> selectedConstructors = constructorSelector.SelectConstructors(instantiableConstructors);
            if (selectedConstructors.Count() == 1)
            {
                selectedConstructor = selectedConstructors.Single();
            }
        }

        return selectedConstructor is not null;
    }
}