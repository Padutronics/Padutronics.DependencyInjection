using System;
using System.Collections.Generic;
using System.Linq;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class AllProfileProvider : IProfileProvider
{
    private readonly Lazy<IEnumerable<Profile>> allProfiles;
    private readonly Lazy<Profile> defaultProfile;

    public AllProfileProvider(IEnumerable<Profile> profiles)
    {
        allProfiles = new Lazy<IEnumerable<Profile>>(() => SelectAllProfiles(profiles));
        defaultProfile = new Lazy<Profile>(() => SelectDefaultProfile(profiles));
    }

    public IEnumerable<Profile> AllProfiles => allProfiles.Value;

    public Profile DefaultProfile => defaultProfile.Value;

    private IEnumerable<Profile> SelectAllProfiles(IEnumerable<Profile> profiles)
    {
        return profiles
            .Where(profile => !profile.IsFallback)
            .ToList();
    }

    private Profile SelectDefaultProfile(IEnumerable<Profile> profiles)
    {
        return profiles.LastOrDefault(profile => !profile.IsFallback)
            ?? profiles.Last(profile => profile.IsFallback);
    }
}