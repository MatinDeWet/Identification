using Identification.Core.Configuration;

namespace Identification.Base.Contracts;

/// <summary>
/// Represents a raw identity claim value with implicit conversions to common ID types.
/// </summary>
public readonly struct IdentityValue
{
    private readonly string _raw;

    /// <summary>
    /// Initializes a new identity value wrapper.
    /// </summary>
    /// <param name="raw">The raw claim value.</param>
    public IdentityValue(string raw)
    {
        _raw = raw ?? string.Empty;
    }

    /// <summary>
    /// Gets the raw claim value.
    /// </summary>
    public string Raw => _raw;

    /// <summary>
    /// Returns the raw string value.
    /// </summary>
    /// <returns>The raw claim value.</returns>
    public override string ToString()
    {
        return _raw;
    }

    /// <summary>
    /// Converts an identity value to a string.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator string(IdentityValue value)
    {
        return value._raw;
    }

    /// <summary>
    /// Converts an identity value to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Guid(IdentityValue value)
    {
        return ClaimValueParsers.Parse<Guid>(value._raw);
    }

    /// <summary>
    /// Converts an identity value to a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator long(IdentityValue value)
    {
        return ClaimValueParsers.Parse<long>(value._raw);
    }

    /// <summary>
    /// Converts an identity value to an <see cref="int"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator int(IdentityValue value)
    {
        return ClaimValueParsers.Parse<int>(value._raw);
    }
}
