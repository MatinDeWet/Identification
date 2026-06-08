using System.Security.Claims;

namespace Identification.Base.Contracts;

public interface IInfoSetter : IList<Claim>
{
    void SetUser(IEnumerable<Claim> claims);
}
