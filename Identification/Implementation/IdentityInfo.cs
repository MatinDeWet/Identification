using System.Security.Claims;
using Identification.Base.Contracts;
using Identification.Core.Configuration;
using Microsoft.Extensions.Options;

namespace Identification.Core.Implementation;

internal class IdentityInfo<TExternalUserId, TInternalUserId> : IIdentityInfo<TExternalUserId, TInternalUserId>
{
    private readonly IInfoSetter _infoSetter;
    private readonly IdentificationOptions<TExternalUserId, TInternalUserId> _options;

    public IdentityInfo(
        IInfoSetter infoSetter,
        IOptions<IdentificationOptions<TExternalUserId, TInternalUserId>> options)
    {
        _infoSetter = infoSetter;
        _options = options.Value;
    }

    public TExternalUserId GetExternalUserId()
    {
        string uid = GetExternalUserIdValue();

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

    public TInternalUserId GetInternalUserId()
    {
        string uid = GetInternalUserIdValue();

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

    public string GetExternalUserIdValue()
    {
        return GetValue(_options.ExternalUserIdClaimType);
    }

    public string GetInternalUserIdValue()
    {
        return GetValue(_options.InternalUserIdClaimType);
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

    IdentityValue IIdentityInfo.GetExternalUserId()
    {
        return new IdentityValue(GetExternalUserIdValue());
    }

    IdentityValue IIdentityInfo.GetInternalUserId()
    {
        return new IdentityValue(GetInternalUserIdValue());
    }

}
