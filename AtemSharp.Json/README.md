# AtemSharp.Json

This library extends [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json) for use with
[AtemSharp](https://www.nuget.org/packages/AtemSharp/).

**Note:** It does not support deserialization

## Usage:

### Add package

Add the package to your project with the following command

```powershell
dotnet add package AtemSharp.Json
```

make sure to use the same version as the one of the `AtemSharp` package.

### Use package

Where you create your JsonSerializerSettings, just append the following:

```C#
var settings = new JsonSerializerSettings { ... }.WithAtemStateSupport();
```

This helper method adds the converters needed to properly serialize the state.

