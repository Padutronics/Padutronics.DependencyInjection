using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

internal sealed class DefaultValueProvider : IValueProvider
{
    public bool CanGetValue(ParameterInfo parameter, IContainer container)
    {
        return parameter.HasDefaultValue;
    }

    public object? GetValue(ParameterInfo parameter, IContainer container)
    {
        return parameter.DefaultValue;
    }
}