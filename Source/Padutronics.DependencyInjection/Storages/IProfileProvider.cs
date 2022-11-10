using System.Collections.Generic;

namespace Padutronics.DependencyInjection.Storages;

internal interface IProfileProvider
{
    IEnumerable<Profile> AllProfiles { get; }
    Profile DefaultProfile { get; }
}