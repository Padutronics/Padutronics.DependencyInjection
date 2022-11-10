namespace Padutronics.DependencyInjection.Storages;

internal sealed class DefaultProfileProvider : IProfileProvider
{
    public DefaultProfileProvider(Profile defaultProfile)
    {
        DefaultProfile = defaultProfile;
    }

    public Profile DefaultProfile { get; }
}