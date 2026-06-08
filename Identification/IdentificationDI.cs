using Identification.Base.Contracts;
using Identification.Core.Configuration;
using Identification.Core.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Identification.Core;

public static class IdentificationDI
{
    public static IServiceCollection AddIdentificationSupport<TEntraId, TUserId>(
        this IServiceCollection services,
        Action<IdentificationOptions<TEntraId, TUserId>> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        services.AddOptions<IdentificationOptions<TEntraId, TUserId>>()
            .Configure(configure)
            .PostConfigure(options =>
        {
            if (string.IsNullOrWhiteSpace(options.ExternalUserIdClaimType))
            {
                throw new InvalidOperationException("ExternalUserIdClaimType must be configured.");
            }

            if (string.IsNullOrWhiteSpace(options.InternalUserIdClaimType))
            {
                throw new InvalidOperationException("InternalUserIdClaimType must be configured.");
            }

            if (string.IsNullOrWhiteSpace(options.RoleClaimType))
            {
                options.RoleClaimType = ClaimTypes.Role;
            }

            if (string.IsNullOrWhiteSpace(options.AdminRoleValue))
            {
                throw new InvalidOperationException("AdminRoleValue must be configured.");
            }

            options.ExternalUserIdParser ??= ClaimValueParsers.Parse<TEntraId>;
            options.InternalUserIdParser ??= ClaimValueParsers.Parse<TUserId>;
        });

        services.AddScoped<IIdentityInfo<TEntraId, TUserId>, IdentityInfo<TEntraId, TUserId>>();
        services.AddScoped<IInfoSetter, InfoSetter>();

        return services;
    }
}
