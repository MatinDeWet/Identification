using System.Security.Claims;
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

    public TEntraId GetExternalUserId()
    {
        string uid = GetValue(_options.ExternalUserIdClaimType);

        if (string.IsNullOrWhiteSpace(uid))
        {
            throw new InvalidOperationException("The external user ID claim is not set.");
        }

        if (_options.ExternalUserIdParser is null)
        {
            throw new InvalidOperationException("ExternalUserIdParser is not configured.");
        }

        return _options.ExternalUserIdParser(uid);
    }

    public TUserId GetInternalUserId()
    {
        string uid = GetValue(_options.InternalUserIdClaimType);

        if (string.IsNullOrWhiteSpace(uid))
        {
            throw new InvalidOperationException("The internal user ID claim is not set.");
        }

        if (_options.InternalUserIdParser is null)
        {
            throw new InvalidOperationException("InternalUserIdParser is not configured.");
        }

        return _options.InternalUserIdParser(uid);
    }

    public bool IsAdmin()
    {
        if (string.IsNullOrWhiteSpace(_options.AdminRoleValue))
        {
            throw new InvalidOperationException("AdminRoleValue is not configured.");
        }

        return HasRole(_options.AdminRoleValue);
    }

    public bool HasRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Role cannot be null, empty, or whitespace.", nameof(role));
        }

        IEnumerable<string> roles = _infoSetter
            .Where(x => x.Type == _options.RoleClaimType)
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
