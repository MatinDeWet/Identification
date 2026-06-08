using System.Security.Claims;

namespace Identification.Base.Constants;

public static class ClaimConstants
{
    public const string EntraId = "http://schemas.microsoft.com/identity/claims/objectidentifier";

    public const string UserId = "user_id";

    public const string Role = ClaimTypes.Role;

    public const string Name = "name";

    public const string Email = "preferred_username";
}
