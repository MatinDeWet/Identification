namespace Identification.Base.Contracts;

/// <summary>
/// Provides access to identity information resolved from configured claims.
/// </summary>
public interface IIdentityInfo
{
    /// <summary>
    /// Gets the external user identifier as a convertible identity value.
    /// </summary>
    /// <returns>The external user identifier value.</returns>
    IdentityValue GetExternalUserId();

    /// <summary>
    /// Gets the internal user identifier as a convertible identity value.
    /// </summary>
    /// <returns>The internal user identifier value.</returns>
    IdentityValue GetInternalUserId();

    /// <summary>
    /// Determines whether the current user has the configured admin role.
    /// </summary>
    /// <returns><c>true</c> when the user is an admin; otherwise, <c>false</c>.</returns>
    bool IsAdmin();

    /// <summary>
    /// Determines whether the current user has a specific role.
    /// </summary>
    /// <param name="role">The role value to check.</param>
    /// <returns><c>true</c> when the user has the role; otherwise, <c>false</c>.</returns>
    bool HasRole(string role);

    /// <summary>
    /// Determines whether a claim with the specified type exists.
    /// </summary>
    /// <param name="name">The claim type name.</param>
    /// <returns><c>true</c> when the claim exists; otherwise, <c>false</c>.</returns>
    bool HasValue(string name);

    /// <summary>
    /// Gets the claim value for a specific claim type.
    /// </summary>
    /// <param name="name">The claim type name.</param>
    /// <returns>The claim value, or an empty string when missing.</returns>
    string GetValue(string name);
}

/// <summary>
/// Strongly typed identity information contract.
/// </summary>
/// <typeparam name="TExternalUserId">The external user identifier type.</typeparam>
/// <typeparam name="TInternalUserId">The internal user identifier type.</typeparam>
public interface IIdentityInfo<TExternalUserId, TInternalUserId> : IIdentityInfo
{
    /// <summary>
    /// Gets the external user identifier using the configured parser.
    /// </summary>
    /// <returns>The parsed external user identifier.</returns>
    new TExternalUserId GetExternalUserId();

    /// <summary>
    /// Gets the internal user identifier using the configured parser.
    /// </summary>
    /// <returns>The parsed internal user identifier.</returns>
    new TInternalUserId GetInternalUserId();
}
