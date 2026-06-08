using Identification.Base.Contracts;
using Identification.Core.Configuration;
using Identification.Core.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Identification.Core;

/// <summary>
/// Dependency injection registration extensions for Identification services.
/// </summary>
public static class IdentificationDI
{
    /// <summary>
    /// Registers Identification services with typed external/internal user IDs.
    /// </summary>
    /// <typeparam name="TExternalUserId">The external user identifier type.</typeparam>
    /// <typeparam name="TInternalUserId">The internal user identifier type.</typeparam>
    /// <param name="services">The DI service collection.</param>
    /// <param name="configure">The options configuration callback.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddIdentificationSupport<TExternalUserId, TInternalUserId>(
        this IServiceCollection services,
        Action<IdentificationOptions<TExternalUserId, TInternalUserId>> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        services.AddOptions<IdentificationOptions<TExternalUserId, TInternalUserId>>()
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

            options.ExternalUserIdParser ??= ClaimValueParsers.Parse<TExternalUserId>;
            options.InternalUserIdParser ??= ClaimValueParsers.Parse<TInternalUserId>;
        });

        services.AddScoped<IIdentityInfo<TExternalUserId, TInternalUserId>, IdentityInfo<TExternalUserId, TInternalUserId>>();
        services.AddScoped<IIdentityInfo>(
            sp => sp.GetRequiredService<IIdentityInfo<TExternalUserId, TInternalUserId>>());
        services.AddScoped<IInfoSetter, InfoSetter>();

        return services;
    }
}
