using System;

namespace Padutronics.DependencyInjection.Storages;

internal sealed class Binding
{
    public Binding(Type serviceType, IProfileProvider profileProvider)
    {
        ProfileProvider = profileProvider;
        ServiceType = serviceType;
    }

    public IProfileProvider ProfileProvider { get; }

    public Type ServiceType { get; }
}