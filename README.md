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

Questions or feedback
=====================
per [at] starcounter.com

Limitations
===========
Check [here](https://github.com/per-samuelsson/GoodGet/issues?labels=enhancement&page=1&state=open).
