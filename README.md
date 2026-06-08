# Identification

A lightweight .NET class library for claim-based identity access and dependency injection wiring.

## Features

- Strongly typed identity access via `IIdentityInfo`
- Claim storage and update contract via `IInfoSetter`
- Simple DI registration with `AddIdentificationSupport`

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

Then consume `IIdentityInfo` where needed.

## Repository

https://github.com/MatinDeWet/Identification
