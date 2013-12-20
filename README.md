What is it
==========
GoodGet lets you specify a set of **NuGet packages** and bring the **latest version** of them down to a **predictable location** on your computer. You can then tell it to check if there is a newer version of any of those packages, and it will **update your outdated packages** to their latest version.

And it's **fast**.

Compared to NuGet.exe
=====================
* NuGet is about **bringing down dependent bits** to your computer.
    * GoodGet is too.
* NuGet brings down bits as **"packages"** from sources called **"nuget feeds"**.
    * GoodGet does too.
* NuGet keeps track of what **version of a package** you've got **installed**.
    * GoodGet does too.
* NuGet **make sure** that **package is installed**.
    * GoodGet does too.
* NuGet allows you to **update the version** to bring down a **newer version** of that package.
    * GoodGet does ***not***.

Instead,

* GoodGet **always bring down the very latest** package version.
    * NuGet does not.
* GoodGet **figure out** whether to **install or update** your package.
    * NuGet does not.
* GoodGet installs the latest version of a package to a **predictable location**. 
    * NuGet, by default, does not.

API
===
GoodGet is exposed both as a **.NET library**

```
PackagesFolder.Install("c:\packages", "NUnit");
```

and as a **command-line** tool.

> C:\Packages>goodget . NUnit

Example command-line usage
==========================
Begin fresh:
```
C:\Users\Per\Temp>dir /b
GoodGet.exe
```

Check usage:
```
C:\Users\Per\Temp>goodget
Usage:
  goodget [options] folder <<package1>, [<package2>, <...>]

Options:
  -verbose  Write verbose output
  -quiet    Write no output
  -time     Prefix all output with time elapsed since starting
  -got      Just show what packages we got

Examples:
  goodget . NUnit
  goodget C:\packages NUnit xUnit jQuery
```

Grab a couple of packages. Use ```-time``` to see the time it takes:
```
C:\Users\Per\Temp>goodget -time packs nunit powerargs
00.0090: Installing nunit from "Official NuGet feed" into packs...
00.0110: NuGet.exe not found. Downloading it...
02.9022: Done (2.6.3 installed)
02.9022: Installing powerargs from "Official NuGet feed" into packs...
05.1650: Done (2.0.4.0-preview installed)
```

Lets see what our folder is now at:
```
C:\Users\Per\Temp>dir /b
GoodGet.exe
nuget.exe
packs

C:\Users\Per\Temp>dir packs /b
nunit
powerargs

C:\Users\Per\Temp>dir packs\nunit /b
lib
license.txt
NUnit.2.6.3.nupkg
```

Now tell GoodGet to check we actually got the latest versions.
This is the real value of what GoodGet are about. Therefore,
do it a few times:
```
C:\Users\Per\Temp>goodget -time packs nunit powerargs
00.8568: All packages up-to-date.

C:\Users\Per\Temp>goodget -time packs nunit powerargs
00.8498: All packages up-to-date.

C:\Users\Per\Temp>goodget -time packs nunit powerargs
00.8436: All packages up-to-date.
```

Questions or feedback
=====================
per [at] starcounter.com

or

file an [issue](https://github.com/per-samuelsson/GoodGet/issues/new).

Limitations
===========
Check [here](https://github.com/per-samuelsson/GoodGet/issues?labels=limitation&page=1&state=open).
