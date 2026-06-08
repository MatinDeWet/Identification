using System.Security.Claims;
using Identification.Base.Contracts;
using Identification.Core;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Identification.UnitTests.UnitTests;

public class IdentityInfoTests
{
    [Fact]
    public void GetExternalAndInternalUserId_ShouldReturnConfiguredTypes()
    {
        ServiceProvider provider = BuildProvider<long, int>();

        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        infoSetter.SetUser(new[]
        {
            new Claim("external_user_id", "123"),
            new Claim("user_id", "42"),
            new Claim(ClaimTypes.Role, "Admin")
        });

        IIdentityInfo<long, int> identityInfo = provider.GetRequiredService<IIdentityInfo<long, int>>();

        identityInfo.GetExternalUserId().ShouldBe(123L);
        identityInfo.GetInternalUserId().ShouldBe(42);
        identityInfo.IsAdmin().ShouldBeTrue();
    }

    [Fact]
    public void NonGenericIdentity_ShouldExposeConvertibleIdentityValue()
    {
        ServiceProvider provider = BuildProvider<long, int>();

        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        infoSetter.SetUser(new[]
        {
            new Claim("external_user_id", "777"),
            new Claim("user_id", "5"),
            new Claim(ClaimTypes.Role, "Judge")
        });

        IIdentityInfo identityInfo = provider.GetRequiredService<IIdentityInfo>();

        long externalUserId = identityInfo.GetExternalUserId();
        int internalUserId = identityInfo.GetInternalUserId();

        externalUserId.ShouldBe(777L);
        internalUserId.ShouldBe(5);
        identityInfo.IsAdmin().ShouldBeFalse();
    }

    [Fact]
    public void HasRole_ShouldBeCaseInsensitive()
    {
        ServiceProvider provider = BuildProvider<long, int>();

        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        infoSetter.SetUser(new[]
        {
            new Claim("external_user_id", "1"),
            new Claim("user_id", "2"),
            new Claim(ClaimTypes.Role, "aDmIn")
        });

        IIdentityInfo<long, int> identityInfo = provider.GetRequiredService<IIdentityInfo<long, int>>();

        identityInfo.HasRole("ADMIN").ShouldBeTrue();
    }

    [Fact]
    public void GetExternalUserId_WhenMissingClaim_ShouldThrow()
    {
        ServiceProvider provider = BuildProvider<long, int>();

        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        infoSetter.SetUser(new[]
        {
            new Claim("user_id", "9"),
            new Claim(ClaimTypes.Role, "Admin")
        });

        IIdentityInfo<long, int> identityInfo = provider.GetRequiredService<IIdentityInfo<long, int>>();

        Should.Throw<InvalidOperationException>(() => identityInfo.GetExternalUserId())
            .Message.ShouldContain("external user ID claim is not set");
    }

    [Fact]
    public void GetInternalUserId_WhenMissingClaim_ShouldThrow()
    {
        ServiceProvider provider = BuildProvider<long, int>();

        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        infoSetter.SetUser(new[]
        {
            new Claim("external_user_id", "100")
        });

        IIdentityInfo<long, int> identityInfo = provider.GetRequiredService<IIdentityInfo<long, int>>();

        Should.Throw<InvalidOperationException>(() => identityInfo.GetInternalUserId())
            .Message.ShouldContain("internal user ID claim is not set");
    }

    [Fact]
    public void HasValueAndGetValue_ShouldReturnExpectedValues()
    {
        ServiceProvider provider = BuildProvider<long, int>();

        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        infoSetter.SetUser(new[]
        {
            new Claim("external_user_id", "100"),
            new Claim("custom", "abc")
        });

        IIdentityInfo identityInfo = provider.GetRequiredService<IIdentityInfo>();

        identityInfo.HasValue("custom").ShouldBeTrue();
        identityInfo.HasValue("missing").ShouldBeFalse();
        identityInfo.GetValue("custom").ShouldBe("abc");
        identityInfo.GetValue("missing").ShouldBe(string.Empty);
    }

    [Fact]
    public void CustomRoleClaimType_ShouldBeUsedForRoleChecks()
    {
        ServiceCollection services = new();

        services.AddIdentificationSupport<long, int>(options =>
        {
            options.ExternalUserIdClaimType = "external_user_id";
            options.InternalUserIdClaimType = "user_id";
            options.RoleClaimType = "custom_role";
            options.AdminRoleValue = "superadmin";
        });

        ServiceProvider provider = services.BuildServiceProvider();

        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        infoSetter.SetUser(new[]
        {
            new Claim("external_user_id", "1"),
            new Claim("user_id", "2"),
            new Claim("custom_role", "SuperAdmin")
        });

        IIdentityInfo<long, int> identityInfo = provider.GetRequiredService<IIdentityInfo<long, int>>();

        identityInfo.IsAdmin().ShouldBeTrue();
    }

    [Fact]
    public void SetUser_ShouldReplacePreviousClaims()
    {
        ServiceProvider provider = BuildProvider<long, int>();
        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        IIdentityInfo identityInfo = provider.GetRequiredService<IIdentityInfo>();

        infoSetter.SetUser(new[] { new Claim("external_user_id", "1") });
        identityInfo.GetExternalUserId().Raw.ShouldBe("1");

        infoSetter.SetUser(new[] { new Claim("external_user_id", "2") });
        identityInfo.GetExternalUserId().Raw.ShouldBe("2");
    }

    [Fact]
    public void HasRole_WithInvalidRole_ShouldThrow()
    {
        ServiceProvider provider = BuildProvider<long, int>();
        IIdentityInfo<long, int> identityInfo = provider.GetRequiredService<IIdentityInfo<long, int>>();

        Should.Throw<ArgumentException>(() => identityInfo.HasRole(" "));
    }

    [Fact]
    public void GetExternalUserId_WhenClaimCannotBeParsed_ShouldThrow()
    {
        ServiceProvider provider = BuildProvider<long, int>();
        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        IIdentityInfo<long, int> identityInfo = provider.GetRequiredService<IIdentityInfo<long, int>>();

        infoSetter.SetUser(new[]
        {
            new Claim("external_user_id", "not-a-number"),
            new Claim("user_id", "2")
        });

        Should.Throw<FormatException>(() => identityInfo.GetExternalUserId());
    }

    [Fact]
    public void HasValue_WithInvalidName_ShouldThrow()
    {
        ServiceProvider provider = BuildProvider<long, int>();
        IIdentityInfo identityInfo = provider.GetRequiredService<IIdentityInfo>();

        Should.Throw<ArgumentException>(() => identityInfo.HasValue(" "));
    }

    [Fact]
    public void GetValue_WithInvalidName_ShouldThrow()
    {
        ServiceProvider provider = BuildProvider<long, int>();
        IIdentityInfo identityInfo = provider.GetRequiredService<IIdentityInfo>();

        Should.Throw<ArgumentException>(() => identityInfo.GetValue(" "));
    }

    private static ServiceProvider BuildProvider<TExternalUserId, TInternalUserId>()
    {
        ServiceCollection services = new();

        services.AddIdentificationSupport<TExternalUserId, TInternalUserId>(options =>
        {
            options.ExternalUserIdClaimType = "external_user_id";
            options.InternalUserIdClaimType = "user_id";
            options.AdminRoleValue = "Admin";
        });

        return services.BuildServiceProvider();
    }
}
