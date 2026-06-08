namespace Identification.Base.Contracts;

public interface IIdentityInfo<TEntraId, TUserId>
{
    TEntraId GetEntraId();

    TUserId GetUserId();

    bool IsAdmin();

    bool HasRole(string role);

    bool HasValue(string name);

    string GetValue(string name);
}

public interface IIdentityInfo : IIdentityInfo<Guid, Guid>
{
}
