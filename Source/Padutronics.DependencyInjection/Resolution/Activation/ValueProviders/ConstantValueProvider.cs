using Padutronics.Diagnostics.Debugging;
using System.Diagnostics;
using System.Reflection;

namespace Padutronics.DependencyInjection.Resolution.Activation.ValueProviders;

[DebuggerDisplay(DebuggerDisplayValues.DebuggerDisplay)]
public abstract class ConstantValueProvider<T> : IValueProvider
{
    private readonly T value;

    protected ConstantValueProvider(T value)
    {
        this.value = value;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"Value = {value}";

    public abstract bool CanGetValue(ParameterInfo parameter, IContainerContext containerContext);

    public object? GetValue(ParameterInfo parameter, IContainerContext containerContext)
    {
        return value;
    }
}