namespace Identification.Core.Configuration;

public sealed class IdentificationOptions<TEntraId, TUserId>
{
    public string ExternalUserIdClaimType { get; set; } = string.Empty;

    public string InternalUserIdClaimType { get; set; } = string.Empty;

    public string RoleClaimType { get; set; } = string.Empty;

    public string AdminRoleValue { get; set; } = string.Empty;

    public Func<string, TEntraId>? ExternalUserIdParser { get; set; }

    public Func<string, TUserId>? InternalUserIdParser { get; set; }
}
