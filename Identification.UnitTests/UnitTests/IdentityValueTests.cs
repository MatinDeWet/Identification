using Identification.Base.Contracts;
using Shouldly;

namespace Identification.UnitTests.UnitTests;

public class IdentityValueTests
{
    [Fact]
    public void IdentityValue_ShouldConvertToSupportedTypes()
    {
        IdentityValue longValue = new("123");
        IdentityValue guidValue = new("9f0b7b09-60c8-4b8b-96ee-2f9017043ec6");

        long convertedLong = longValue;
        int convertedInt = new IdentityValue("12");
        Guid convertedGuid = guidValue;
        string convertedString = longValue;

        convertedLong.ShouldBe(123L);
        convertedInt.ShouldBe(12);
        convertedGuid.ShouldBe(Guid.Parse("9f0b7b09-60c8-4b8b-96ee-2f9017043ec6"));
        convertedString.ShouldBe("123");
    }

    [Fact]
    public void IdentityValue_WithInvalidGuid_ShouldThrow()
    {
        IdentityValue value = new("not-a-guid");

        Should.Throw<InvalidOperationException>(() =>
        {
            Guid _ = value;
        });
    }

    [Fact]
    public void IdentityValue_WithInvalidLong_ShouldThrow()
    {
        IdentityValue value = new("abc");

        Should.Throw<FormatException>(() =>
        {
            long _ = value;
        });
    }
}
