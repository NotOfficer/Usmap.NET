<div align="center">

# Usmap.NET

A .NET parser for .usmap files

[![GitHub release](https://img.shields.io/github/v/release/NotOfficer/Usmap.NET?logo=github)](https://github.com/NotOfficer/Usmap.NET/releases/latest) [![Nuget](https://img.shields.io/nuget/v/Usmap.NET?logo=nuget)](https://www.nuget.org/packages/Usmap.NET) [![Nuget DLs](https://img.shields.io/nuget/dt/Usmap.NET?logo=nuget)](https://www.nuget.org/packages/Usmap.NET) [![GitHub issues](https://img.shields.io/github/issues/NotOfficer/Usmap.NET?logo=github)](https://github.com/NotOfficer/Usmap.NET/issues) [![GitHub License](https://img.shields.io/github/license/NotOfficer/Usmap.NET)](https://github.com/NotOfficer/Usmap.NET/blob/master/LICENSE)

</div>

## Example Usage

```cs
using UsmapDotNet;

var usmap = Usmap.Parse("C:/Test/Example.usmap", new UsmapOptions
{
    //Oodle = oodleInstance,
    SaveNames = false
});
```

### Info

The `UsmapOptions.Oodle` instance is only required for loading oodle compressed usmap files.  
Oodle decompression has been tested on windows and linux (x64).

### NuGet

```md
Install-Package Usmap.NET
```

### Contribute

If you can provide any help, may it only be spell checking please contribute!  
I am open for any contribution.
