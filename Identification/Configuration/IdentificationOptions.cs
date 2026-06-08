namespace Identification.Core.Configuration;

/// <summary>
/// Configuration options for claim mapping and parsing behavior.
/// </summary>
/// <typeparam name="TExternalUserId">The external user identifier type.</typeparam>
/// <typeparam name="TInternalUserId">The internal user identifier type.</typeparam>
public sealed class IdentificationOptions<TExternalUserId, TInternalUserId>
{
    /// <summary>
    /// Claim type used to read the external user identifier.
    /// </summary>
    public string ExternalUserIdClaimType { get; set; } = string.Empty;

    /// <summary>
    /// Claim type used to read the internal user identifier.
    /// </summary>
    public string InternalUserIdClaimType { get; set; } = string.Empty;

    /// <summary>
    /// Claim type used for role checks.
    /// </summary>
    public string RoleClaimType { get; set; } = string.Empty;

    /// <summary>
    /// Role value treated as admin.
    /// </summary>
    public string AdminRoleValue { get; set; } = string.Empty;

    /// <summary>
    /// Parser used to convert the external user ID claim value.
    /// </summary>
    public Func<string, TExternalUserId>? ExternalUserIdParser { get; set; }

    /// <summary>
    /// Parser used to convert the internal user ID claim value.
    /// </summary>
    public Func<string, TInternalUserId>? InternalUserIdParser { get; set; }
}
