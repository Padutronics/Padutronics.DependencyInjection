namespace Padutronics.DependencyInjection;

public interface IContainerModule
{
    void Load(IContainerBuilder containerBuilder);
}