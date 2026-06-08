# Identification

A lightweight .NET class library for claim-based identity access with configurable ID parsing and simple DI wiring.

## Features

- Read user identity data from claims through one interface.
- Configure claim names and parsing logic in DI.
- Use strongly typed IDs with `IIdentityInfo<TExternalUserId, TInternalUserId>`.
- Or inject the simpler non-generic `IIdentityInfo` when preferred.

## Installation

```bash
dotnet add package MatinDeWet.Identification
```

## Quick Start

1. Register the package in DI (explicit configuration is required).
2. Populate `IInfoSetter` from the current request claims.
3. Inject `IIdentityInfo` or `IIdentityInfo<TExternalUserId, TInternalUserId>` in your services.

### 1) Register In DI

```csharp
using Identification.Base.Contracts;
using Identification.Core;

services.AddIdentificationSupport<long, int>(options =>
{
	options.ExternalUserIdClaimType = "external_user_id";
	options.InternalUserIdClaimType = "user_id";
	options.RoleClaimType = "role";
	options.AdminRoleValue = "Admin";
	options.ExternalUserIdParser = value => long.Parse(value);
	options.InternalUserIdParser = value => int.Parse(value);
});
```

### 2) Populate Claim Data Per Request

The package reads claims from `IInfoSetter`, so set those claims early in the pipeline.

```csharp
using Identification.Base.Contracts;

app.Use(async (context, next) =>
{
	IInfoSetter infoSetter = context.RequestServices.GetRequiredService<IInfoSetter>();
	infoSetter.SetUser(context.User.Claims);
	await next();
});
```

### 3) Inject And Use

Typed usage:

```csharp
using Identification.Base.Contracts;

public sealed class TypedIdentityService
{
	private readonly IIdentityInfo<long, int> _identityInfo;

	public TypedIdentityService(IIdentityInfo<long, int> identityInfo)
	{
		_identityInfo = identityInfo;
	}

	public (long externalId, int internalId, bool isAdmin) ReadIdentity()
	{
		long externalId = _identityInfo.GetExternalUserId();
		int internalId = _identityInfo.GetInternalUserId();
		bool isAdmin = _identityInfo.IsAdmin();

		return (externalId, internalId, isAdmin);
	}
}
```

Non-generic usage:

```csharp
using Identification.Base.Contracts;

public sealed class MyService
{
	private readonly IIdentityInfo _identityInfo;

	public MyService(IIdentityInfo identityInfo)
	{
		_identityInfo = identityInfo;
	}

	public bool CanAccessAdmin()
	{
		return _identityInfo.IsAdmin();
	}

	public long GetExternalId()
	{
		long externalId = _identityInfo.GetExternalUserId();
		return externalId;
	}
}
```

You can also read as other supported types directly:

```csharp
Guid externalId = identityInfo.GetExternalUserId();
int internalId = identityInfo.GetInternalUserId();
string externalIdRaw = identityInfo.GetExternalUserId();
```

## Configuration Reference

`IdentificationOptions<TExternalUserId, TInternalUserId>`

- `ExternalUserIdClaimType`: Claim type name for external user ID. Required.
- `InternalUserIdClaimType`: Claim type name for internal user ID. Required.
- `RoleClaimType`: Claim type used for role checks. Optional. Defaults to `ClaimTypes.Role` if not set.
- `AdminRoleValue`: Role value considered admin. Required.
- `ExternalUserIdParser`: Converts claim value into `TExternalUserId`. Optional. Default parser is provided.
- `InternalUserIdParser`: Converts claim value into `TInternalUserId`. Optional. Default parser is provided.

## Public Interface Methods

`IIdentityInfo<TExternalUserId, TInternalUserId>`

- `GetExternalUserId()`
- `GetInternalUserId()`
- `IsAdmin()`
- `HasRole(string role)`
- `HasValue(string claimType)`
- `GetValue(string claimType)`

`IIdentityInfo`

- `GetExternalUserId()` returns `IdentityValue` (implicitly convertible to `string`, `Guid`, `long`, `int`)
- `GetInternalUserId()` returns `IdentityValue` (implicitly convertible to `string`, `Guid`, `long`, `int`)
- `IsAdmin()`
- `HasRole(string role)`
- `HasValue(string claimType)`
- `GetValue(string claimType)`

## Notes

- If claim values are missing or invalid for your parser, methods throw `InvalidOperationException`.
- Keep your parser functions aligned with the claim formats emitted by your identity provider.
- Prefer typed interface injection when you want compile-time ID types.

## Compile-Time Safety Without Repeating Generics

If you want compile-time safety and still inject `IIdentityInfo`-style names in your app code, create a global alias in your consuming project:

```csharp
// GlobalUsings.cs in your application
global using IIdentityInfo = Identification.Base.Contracts.IIdentityInfo<long, int>;
```

Then in services/controllers you can use:

```csharp
private readonly IIdentityInfo _identityInfo;
```

This keeps compile-time typed IDs while avoiding repeated generic type arguments everywhere.

## Repository

https://github.com/MatinDeWet/Identification
