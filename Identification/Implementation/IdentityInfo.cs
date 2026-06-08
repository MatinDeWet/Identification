using System.Security.Claims;
using Identification.Base.Constants;
using Identification.Base.Contracts;
using Identification.Core.Configuration;
using Microsoft.Extensions.Options;

namespace Identification.Core.Implementation;

internal class IdentityInfo<TEntraId, TUserId> : IIdentityInfo<TEntraId, TUserId>
{
    private readonly IInfoSetter _infoSetter;
    private readonly IdentificationOptions<TEntraId, TUserId> _options;

    public IdentityInfo(
        IInfoSetter infoSetter,
        IOptions<IdentificationOptions<TEntraId, TUserId>> options)
    {
        _infoSetter = infoSetter;
        _options = options.Value;
    }

    public TEntraId GetEntraId()
    {
        string uid = GetValue(_options.EntraIdClaimType);

        if (string.IsNullOrWhiteSpace(uid))
        {
            throw new InvalidOperationException("The Entra ID claim is not set.");
        }

        return _options.EntraIdParser(uid);
    }

    public TUserId GetUserId()
    {
        string uid = GetValue(_options.UserIdClaimType);

        if (string.IsNullOrWhiteSpace(uid))
        {
            throw new InvalidOperationException("The user ID claim is not set.");
        }

        return _options.UserIdParser(uid);
    }

    public bool IsAdmin()
    {
        return HasRole(RoleConstants.Admin);
    }

    public bool HasRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Role cannot be null, empty, or whitespace.", nameof(role));
        }

        IEnumerable<string> roles = _infoSetter
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return roles.Any(roleString =>
            roleString.Equals(role, StringComparison.OrdinalIgnoreCase));
    }

    public string GetValue(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(name));
        }

        Claim? claim = _infoSetter.FirstOrDefault(x => x.Type == name);
        return claim?.Value ?? string.Empty;
    }

    public bool HasValue(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(name));
        }

        return _infoSetter.Any(x => x.Type == name);
    }
}

internal sealed class IdentityInfo : IdentityInfo<Guid, Guid>, IIdentityInfo
{
    public IdentityInfo(
        IInfoSetter infoSetter,
        IOptions<IdentificationOptions<Guid, Guid>> options)
        : base(infoSetter, options)
    {
    }
}
