using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

public sealed class TypedValueProvider<T> : ConstantValueProvider<T>
{
    public TypedValueProvider(T value) :
        base(value)
    {
    }

    public override bool CanGetValue(ParameterInfo parameter, IContainerContext containerContext)
    {
        return parameter.ParameterType == typeof(T);
    }
}