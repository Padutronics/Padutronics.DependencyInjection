using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

public sealed class NamedValueProvider<T> : ConstantValueProvider<T>
{
    private readonly string name;

    public NamedValueProvider(string name, T value) :
        base(value)
    {
        this.name = name;
    }

    public override bool CanGetValue(ParameterInfo parameter, IContainerContext containerContext)
    {
        return parameter.Name == name;
    }
}