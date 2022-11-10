using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class DefaultProfileProvider : IProfileProvider
{
    public DefaultProfileProvider(Profile defaultProfile)
    {
        DefaultProfile = defaultProfile;
    }

    public IEnumerable<Profile> AllProfiles => new[] { DefaultProfile };

    public Profile DefaultProfile { get; }
}