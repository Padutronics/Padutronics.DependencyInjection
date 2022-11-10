using Padutronics.DependencyInjection.Resolution.Activation;

namespace Padutronics.DependencyInjection.Resolution;

internal sealed class ConstantInstanceProvider<TImplementation> : IInstanceProvider<TImplementation>
    where TImplementation : class
{
    private readonly TImplementation constant;

    public ConstantInstanceProvider(TImplementation constant)
    {
        this.constant = constant;
    }

    public TImplementation GetInstance(ActivationSession session)
    {
        return constant;
    }
}