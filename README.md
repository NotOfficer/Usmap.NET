﻿<div align="center">

# 🚀 Usmap.NET

**A .NET parser for .usmap files**  

[![GitHub release](https://img.shields.io/github/v/release/NotOfficer/Usmap.NET?logo=github)](https://github.com/NotOfficer/Usmap.NET/releases/latest)
[![Nuget](https://img.shields.io/nuget/v/Usmap.NET?logo=nuget)](https://www.nuget.org/packages/Usmap.NET)
[![Nuget Downloads](https://img.shields.io/nuget/dt/Usmap.NET?logo=nuget)](https://www.nuget.org/packages/Usmap.NET)
[![GitHub issues](https://img.shields.io/github/issues/NotOfficer/Usmap.NET?logo=github)](https://github.com/NotOfficer/Usmap.NET/issues)
[![License](https://img.shields.io/github/license/NotOfficer/Usmap.NET)](https://github.com/NotOfficer/Usmap.NET/blob/master/LICENSE)

</div>

---

## 📦 Installation

Install via [NuGet](https://www.nuget.org/packages/Usmap.NET):

```powershell
Install-Package Usmap.NET
```

---

## ✨ Features

- Supports up to version **4** (`ExplicitEnumValues`)
- Supports compressed files with: **Brotli**, **Oodle** & **Zstandard**

---

## 🔧 Example Usage

```cs
using UsmapDotNet;

var usmap = Usmap.Parse("C:/Test/Example.usmap", new UsmapOptions
{
    Oodle = oodleInstance, // optional for oodle compressed files
    SaveNames = false
});
```

---

## 🤝 Contributing

Contributions are **welcome and appreciated**!

Whether it's fixing a typo, suggesting an improvement, or submitting a pull request — every bit helps.

---

## 📄 License

This project is licensed under the [MIT License](https://github.com/NotOfficer/Usmap.NET/blob/master/LICENSE).

---

<div align="center">

⭐️ Star the repo if you find it useful!  
Feel free to open an issue if you have any questions or feedback.

</div>
