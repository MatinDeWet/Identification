using Identification.Core.Configuration;

namespace Identification.Base.Contracts;

public readonly struct IdentityValue
{
    private readonly string _raw;

    public IdentityValue(string raw)
    {
        _raw = raw ?? string.Empty;
    }

    public string Raw => _raw;

    public override string ToString()
    {
        return _raw;
    }

    public static implicit operator string(IdentityValue value)
    {
        return value._raw;
    }

    public static implicit operator Guid(IdentityValue value)
    {
        return ClaimValueParsers.Parse<Guid>(value._raw);
    }

    public static implicit operator long(IdentityValue value)
    {
        return ClaimValueParsers.Parse<long>(value._raw);
    }

    public static implicit operator int(IdentityValue value)
    {
        return ClaimValueParsers.Parse<int>(value._raw);
    }
}
