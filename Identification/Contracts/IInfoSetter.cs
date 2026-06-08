using System.Security.Claims;

namespace Identification.Base.Contracts;

/// <summary>
/// Stores the active claim set used by identity resolution services.
/// </summary>
public interface IInfoSetter : IList<Claim>
{
    /// <summary>
    /// Replaces the current claim set.
    /// </summary>
    /// <param name="claims">The claims to store for subsequent identity reads.</param>
    void SetUser(IEnumerable<Claim> claims);
}
