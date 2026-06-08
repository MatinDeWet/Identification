# Identification

A lightweight .NET class library for claim-based identity access and dependency injection wiring.

## Features

- Strongly typed identity access via `IIdentityInfo<TEntraId, TUserId>`
- Claim storage and update contract via `IInfoSetter`
- DI registration with configurable claim names and parsers

## Installation

```bash
dotnet add package MatinDeWet.Identification
```

## Usage

Register services:

```csharp
using Identification.Core;

services.AddIdentificationSupport();
```

Use custom claim names and output types:

```csharp
using Identification.Base.Contracts;
using Identification.Core;

services.AddIdentificationSupport<long, int>(options =>
{
	options.EntraIdClaimType = "entra_id";
	options.UserIdClaimType = "user_id";
	options.EntraIdParser = value => long.Parse(value);
	options.UserIdParser = value => int.Parse(value);
});

// Inject IIdentityInfo<long, int>
```

Default behavior remains available for Guid-based IDs via `AddIdentificationSupport()` and `IIdentityInfo`.

## Repository

https://github.com/MatinDeWet/Identification
