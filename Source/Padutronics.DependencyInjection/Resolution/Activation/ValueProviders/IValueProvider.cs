using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

internal interface IValueProvider
{
    bool CanGetValue(ParameterInfo parameter, IContainer container);
    object? GetValue(ParameterInfo parameter, IContainer container);
}