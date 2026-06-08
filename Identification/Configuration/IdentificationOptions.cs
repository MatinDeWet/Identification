namespace Identification.Core.Configuration;

public sealed class IdentificationOptions<TExternalUserId, TInternalUserId>
{
    public string ExternalUserIdClaimType { get; set; } = string.Empty;

    public string InternalUserIdClaimType { get; set; } = string.Empty;

    public string RoleClaimType { get; set; } = string.Empty;

    public string AdminRoleValue { get; set; } = string.Empty;

    public Func<string, TExternalUserId>? ExternalUserIdParser { get; set; }

    public Func<string, TInternalUserId>? InternalUserIdParser { get; set; }
}
