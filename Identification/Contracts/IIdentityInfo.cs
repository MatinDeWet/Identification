namespace Identification.Base.Contracts;

public interface IIdentityInfo
{
    Guid GetEntraId();

    Guid GetUserId();

    bool IsAdmin();

    bool HasRole(string role);

    bool HasValue(string name);

    string GetValue(string name);
}
