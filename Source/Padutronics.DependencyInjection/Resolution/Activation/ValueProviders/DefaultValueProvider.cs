using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

internal sealed class DefaultValueProvider : IValueProvider
{
    public bool CanGetValue(ParameterInfo parameter, IContainerContext containerContext)
    {
        return parameter.HasDefaultValue;
    }

    public object? GetValue(ParameterInfo parameter, IContainerContext containerContext)
    {
        return parameter.DefaultValue;
    }
}