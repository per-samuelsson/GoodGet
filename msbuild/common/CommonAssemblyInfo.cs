using System;
using System.Reflection;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyProduct("Starbits")]
[assembly: AssemblyCopyright("Copyright 2013 Starcounter")]

[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]