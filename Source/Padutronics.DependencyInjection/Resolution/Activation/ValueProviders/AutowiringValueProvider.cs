using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

internal sealed class AutowiringValueProvider : IValueProvider
{
    public bool CanGetValue(ParameterInfo parameter, IContainer container)
    {
        return container.CanResolve(parameter.ParameterType);
    }

    public object? GetValue(ParameterInfo parameter, IContainer container)
    {
        return container.Resolve(parameter.ParameterType);
    }
}