using System.Globalization;

namespace Identification.Core.Configuration;

public static class ClaimValueParsers
{
    public static T Parse<T>(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        object parsed = targetType switch
        {
            _ when targetType == typeof(string) => value,
            _ when targetType == typeof(Guid) => ParseGuid(value),
            _ when targetType == typeof(int) => int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture),
            _ when targetType == typeof(long) => long.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture),
            _ when targetType == typeof(short) => short.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture),
            _ when targetType == typeof(byte) => byte.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture),
            _ when targetType == typeof(bool) => bool.Parse(value),
            _ when targetType == typeof(decimal) => decimal.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture),
            _ when targetType == typeof(double) => double.Parse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
            _ when targetType == typeof(float) => float.Parse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
            _ when targetType.IsEnum => Enum.Parse(targetType, value, ignoreCase: true),
            _ => Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture)
        };

        return (T)parsed;
    }

    private static Guid ParseGuid(string value)
    {
        if (!Guid.TryParse(value, out Guid guid))
        {
            throw new InvalidOperationException($"Unable to parse '{value}' as a Guid.");
        }

        return guid;
    }
}
