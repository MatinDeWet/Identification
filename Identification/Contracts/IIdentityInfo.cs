namespace Identification.Base.Contracts;

public interface IIdentityInfo<TEntraId, TUserId>
{
    TEntraId GetExternalUserId();

    TUserId GetInternalUserId();

    bool IsAdmin();

    bool HasRole(string role);

    bool HasValue(string name);

    string GetValue(string name);
}
