namespace Identification.Base.Contracts;

public interface IIdentityInfo<TExternalUserId, TInternalUserId>
{
    TExternalUserId GetExternalUserId();

    TInternalUserId GetInternalUserId();

    bool IsAdmin();

    bool HasRole(string role);

    bool HasValue(string name);

    string GetValue(string name);
}
