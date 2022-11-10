using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

internal sealed class AutowiringValueProvider : IValueProvider
{
    public bool CanGetValue(ParameterInfo parameter, IContainerContext containerContext)
    {
        return containerContext.CanResolve(parameter.ParameterType);
    }

    public object? GetValue(ParameterInfo parameter, IContainerContext containerContext)
    {
        return containerContext.Resolve(parameter.ParameterType);
    }
}