using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("GoodGet")]
[assembly: AssemblyDescription("Continous integration NuGet variant")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Starcounter")]
[assembly: AssemblyProduct("GoodGet")]
[assembly: AssemblyCopyright("Copyright © Starcounter 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f471abde-57d7-419c-9328-c70bbc9b1454")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0")]
[assembly: AssemblyFileVersion("1.0.0")]

// The current, "real" version. Will be target to the
// CI build SemVer extension, and generate packages like
// <this_value>.[n].[time] where [n] is an integer that
// increase for every version we publish, and [time] is
// the time the package was built/packaged. This will ONLY
// apply as long as we are in prerelease mode. After that,
// the package/assembly will be stable enough to only be
// updated manually.
// Example extended SemVer: 1.0.0-alpha.123.20131220T065113
[assembly: AssemblyInformationalVersionAttribute("1.0.0-alpha")]