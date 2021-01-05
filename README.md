# Usmap.NET
.NET parser for .usmap files

<div align="center">

# Usmap.NET

[![GitHub release](https://img.shields.io/github/v/release/NotOfficer/Usmap.NET?logo=github)](https://github.com/NotOfficer/Usmap.NET/releases/latest) [![Nuget](https://img.shields.io/nuget/v/Usmap.NET?logo=nuget)](https://www.nuget.org/packages/Usmap.NET) [![Nuget DLs](https://img.shields.io/nuget/dt/Usmap.NET?logo=nuget)](https://www.nuget.org/packages/Usmap.NET) [![GitHub issues](https://img.shields.io/github/issues/NotOfficer/Usmap.NET?logo=github)](https://github.com/NotOfficer/Usmap.NET/issues) [![GitHub License](https://img.shields.io/github/license/NotOfficer/Usmap.NET)](https://github.com/NotOfficer/Usmap.NET/blob/master/LICENSE)

</div>

A .NET parser for .usmap files.

## NuGet

    Install-Package Usmap.NET

## Example Usage

```cs
using UsmapNET;

var usmap = new Usmap(@"C:\Downloads\Example.usmap");
```

If you want to parse oodle comrpessed usmap files, you will need to have the oodle library renamed to `oo2core64` in your working directory.

### Contribute

If you can provide any help, may it only be spell checking please contribute!
I am open for any contribution.