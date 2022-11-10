using System;
using System.Collections.Generic;
using System.Linq;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class AllProfileProvider : IProfileProvider
{
    private readonly Lazy<Profile> defaultProfile;

    public AllProfileProvider(IEnumerable<Profile> profiles)
    {
        defaultProfile = new Lazy<Profile>(() => SelectDefaultProfile(profiles));
    }

    public Profile DefaultProfile => defaultProfile.Value;

    private Profile SelectDefaultProfile(IEnumerable<Profile> profiles)
    {
        return profiles.LastOrDefault(profile => !profile.IsFallback)
            ?? profiles.Last(profile => profile.IsFallback);
    }
}