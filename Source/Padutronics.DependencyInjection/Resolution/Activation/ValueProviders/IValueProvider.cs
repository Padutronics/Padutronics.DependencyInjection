using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

public interface IValueProvider
{
    bool CanGetValue(ParameterInfo parameter, IContainerContext containerContext);
    object? GetValue(ParameterInfo parameter, IContainerContext containerContext);
}