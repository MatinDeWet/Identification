using Identification.Base.Contracts;
using Identification.Core;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Identification.UnitTests.UnitTests;

public class IdentificationDITests
{
    [Fact]
    public void AddIdentificationSupport_WithMissingExternalClaimType_ShouldThrowOnResolve()
    {
        ServiceCollection services = new();

        services.AddIdentificationSupport<long, int>(options =>
        {
            options.InternalUserIdClaimType = "user_id";
            options.AdminRoleValue = "Admin";
        });

        ServiceProvider provider = services.BuildServiceProvider();

        Should.Throw<InvalidOperationException>(() => provider.GetRequiredService<IIdentityInfo<long, int>>())
            .Message.ShouldContain("ExternalUserIdClaimType must be configured");
    }

    [Fact]
    public void AddIdentificationSupport_WithMissingAdminRoleValue_ShouldThrowOnResolve()
    {
        ServiceCollection services = new();

        services.AddIdentificationSupport<long, int>(options =>
        {
            options.ExternalUserIdClaimType = "external_user_id";
            options.InternalUserIdClaimType = "user_id";
        });

        ServiceProvider provider = services.BuildServiceProvider();

        Should.Throw<InvalidOperationException>(() => provider.GetRequiredService<IIdentityInfo<long, int>>())
            .Message.ShouldContain("AdminRoleValue must be configured");
    }

    [Fact]
    public void AddIdentificationSupport_WithMissingInternalClaimType_ShouldThrowOnResolve()
    {
        ServiceCollection services = new();

        services.AddIdentificationSupport<long, int>(options =>
        {
            options.ExternalUserIdClaimType = "external_user_id";
            options.AdminRoleValue = "Admin";
        });

        ServiceProvider provider = services.BuildServiceProvider();

        Should.Throw<InvalidOperationException>(() => provider.GetRequiredService<IIdentityInfo<long, int>>())
            .Message.ShouldContain("InternalUserIdClaimType must be configured");
    }

    [Fact]
    public void AddIdentificationSupport_WithNullConfigure_ShouldThrow()
    {
        ServiceCollection services = new();

        Should.Throw<ArgumentNullException>(() =>
            services.AddIdentificationSupport<long, int>(null!));
    }

    [Fact]
    public void AddIdentificationSupport_WhenRoleClaimTypeMissing_ShouldDefaultToClaimTypesRole()
    {
        ServiceCollection services = new();

        services.AddIdentificationSupport<long, int>(options =>
        {
            options.ExternalUserIdClaimType = "external_user_id";
            options.InternalUserIdClaimType = "user_id";
            options.AdminRoleValue = "Admin";
        });

        ServiceProvider provider = services.BuildServiceProvider();
        IInfoSetter infoSetter = provider.GetRequiredService<IInfoSetter>();
        IIdentityInfo<long, int> identityInfo = provider.GetRequiredService<IIdentityInfo<long, int>>();

        infoSetter.SetUser(new[]
        {
            new Claim("external_user_id", "1"),
            new Claim("user_id", "2"),
            new Claim(ClaimTypes.Role, "admin")
        });

        identityInfo.IsAdmin().ShouldBeTrue();
    }

    [Fact]
    public void AddIdentificationSupport_ShouldRegisterNonGenericAlias()
    {
        ServiceCollection services = new();

        services.AddIdentificationSupport<long, int>(options =>
        {
            options.ExternalUserIdClaimType = "external_user_id";
            options.InternalUserIdClaimType = "user_id";
            options.AdminRoleValue = "Admin";
        });

        ServiceProvider provider = services.BuildServiceProvider();

        IIdentityInfo nonGeneric = provider.GetRequiredService<IIdentityInfo>();
        nonGeneric.ShouldNotBeNull();
    }
}
