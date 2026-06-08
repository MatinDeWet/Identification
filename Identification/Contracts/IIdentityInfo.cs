namespace Identification.Base.Contracts;

public interface IIdentityInfo
{
    object GetExternalUserId();

    object GetInternalUserId();

    bool IsAdmin();

    bool HasRole(string role);

    bool HasValue(string name);

    string GetValue(string name);
}

public interface IIdentityInfo<TExternalUserId, TInternalUserId> : IIdentityInfo
{
    new TExternalUserId GetExternalUserId();

    new TInternalUserId GetInternalUserId();
}
