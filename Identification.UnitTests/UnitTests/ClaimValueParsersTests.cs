using Identification.Core.Configuration;
using Shouldly;

namespace Identification.UnitTests.UnitTests;

public class ClaimValueParsersTests
{
    private enum SampleEnum
    {
        None,
        Admin,
        User
    }

    [Fact]
    public void Parse_ShouldHandleCommonTypes()
    {
        ClaimValueParsers.Parse<int>("12").ShouldBe(12);
        ClaimValueParsers.Parse<long>("120").ShouldBe(120L);
        ClaimValueParsers.Parse<bool>("true").ShouldBeTrue();
        ClaimValueParsers.Parse<string>("abc").ShouldBe("abc");
        ClaimValueParsers.Parse<SampleEnum>("admin").ShouldBe(SampleEnum.Admin);
    }

    [Fact]
    public void Parse_WithInvalidGuid_ShouldThrow()
    {
        Should.Throw<InvalidOperationException>(() => ClaimValueParsers.Parse<Guid>("not-a-guid"));
    }

    [Fact]
    public void Parse_WithWhitespace_ShouldThrow()
    {
        Should.Throw<ArgumentException>(() => ClaimValueParsers.Parse<int>(" "));
    }
}
