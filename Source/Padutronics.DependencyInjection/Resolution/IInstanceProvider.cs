using Padutronics.DependencyInjection.Resolution.Activation;

namespace Padutronics.DependencyInjection.Resolution;

internal interface IInstanceProvider<out TImplementation>
    where TImplementation : class
{
    TImplementation GetInstance(ActivationSession session);
}