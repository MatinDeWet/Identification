using Identification.Base.Constants;

namespace Identification.Core.Configuration;

public sealed class IdentificationOptions<TEntraId, TUserId>
{
    public string EntraIdClaimType { get; set; } = ClaimConstants.EntraId;

    public string UserIdClaimType { get; set; } = ClaimConstants.UserId;

    public Func<string, TEntraId> EntraIdParser { get; set; } = ClaimValueParsers.Parse<TEntraId>;

    public Func<string, TUserId> UserIdParser { get; set; } = ClaimValueParsers.Parse<TUserId>;
}
