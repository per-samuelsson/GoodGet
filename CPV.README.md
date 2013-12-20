Continuous Package Versioning (CPV)
===================================
Continuous Package Versioning is an auto-incrementing version algorithm based on, and not
violating, SemVer (semver.org), designed for continuous builds. It exploiting the capabilities
of the prerelease string, and hence is only applicable to version numbers that

1. is based on SemVer
2. represents a prerelease artifact

Its generic
===========
The algorithm is generic in that sense that it can be used to version anything,
not just packages. But since our use of it is to version continuously built NuGet
packages, the name is derived from that.

Availability
============
The implementation will be available as a .NET library, a command-line tool, as
an inline MsBuild task and as a compiled MsBuild dito.

Questions or feedback
=====================
per [at] starcounter.com