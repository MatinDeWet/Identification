# Identification

A lightweight .NET class library for claim-based identity access and dependency injection wiring.

## Features

- Strongly typed identity access via `IIdentityInfo<TExternalUserId, TInternalUserId>`
- Claim storage and update contract via `IInfoSetter`
- Explicit DI registration with configurable claim names, admin role, and parsers

## Installation

```bash
dotnet add package MatinDeWet.Identification
```

## Usage

Register services explicitly (required):

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

// Inject IIdentityInfo<long, int>
```

Methods on `IIdentityInfo<TExternalUserId, TInternalUserId>`:

- `GetExternalUserId()`
- `GetInternalUserId()`
- `IsAdmin()`

## Repository

https://github.com/MatinDeWet/Identification
