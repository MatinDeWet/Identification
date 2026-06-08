using Identification.Base.Contracts;
using Identification.Core.Configuration;
using Identification.Core.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Identification.Core;

public static class IdentificationDI
{
    public static IServiceCollection AddIdentificationSupport(this IServiceCollection services)
    {
        return services.AddIdentificationSupport<Guid, Guid>();
    }

    public static IServiceCollection AddIdentificationSupport<TEntraId, TUserId>(
        this IServiceCollection services,
        Action<IdentificationOptions<TEntraId, TUserId>>? configure = null)
    {
        services.AddOptions<IdentificationOptions<TEntraId, TUserId>>();

        if (configure is not null)
        {
            services.Configure(configure);
        }

        services.AddScoped<IIdentityInfo<TEntraId, TUserId>, IdentityInfo<TEntraId, TUserId>>();
        services.AddScoped<IInfoSetter, InfoSetter>();

        if (typeof(TEntraId) == typeof(Guid) && typeof(TUserId) == typeof(Guid))
        {
            services.AddScoped<IIdentityInfo>(
                sp => (IIdentityInfo)sp.GetRequiredService<IIdentityInfo<TEntraId, TUserId>>());
        }

        return services;
    }
}
