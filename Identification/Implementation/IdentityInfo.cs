using System.Security.Claims;
using Identification.Base.Constants;
using Identification.Base.Contracts;

namespace Identification.Core.Implementation;

internal sealed class IdentityInfo : IIdentityInfo
{
    private readonly IInfoSetter _infoSetter;

    public IdentityInfo(IInfoSetter infoSetter)
    {
        _infoSetter = infoSetter;
    }

    public Guid GetEntraId()
    {
        string uid = GetValue(ClaimConstants.EntraId);

        if (string.IsNullOrWhiteSpace(uid) || !Guid.TryParse(uid, out Guid result))
        {
            throw new InvalidOperationException("The system identity ID is not set or is not valid.");
        }

        return result;
    }

    public Guid GetUserId()
    {
        string uid = GetValue(ClaimConstants.UserId);

        if (string.IsNullOrWhiteSpace(uid) || !Guid.TryParse(uid, out Guid result))
        {
            throw new InvalidOperationException("The system identity ID is not set or is not valid.");
        }

        return result;
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
