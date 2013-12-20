
using System;
using System.Diagnostics;

namespace ContinuousPackageVersioning.UnitTests {

    internal static class Test {

        public static void IsCompatible() {
            Trace.Assert(!Version.IsCompatible(null));
            Trace.Assert(!Version.IsCompatible(string.Empty));
            Trace.Assert(!Version.IsCompatible("a"));
            Trace.Assert(!Version.IsCompatible("a.b.c"));
            Trace.Assert(!Version.IsCompatible("a.b.c-d-e"));
            Trace.Assert(!Version.IsCompatible("a.b.c-d+e"));
            Trace.Assert(!Version.IsCompatible("1.2.3-alpha+123"));

            Trace.Assert(Version.IsCompatible("1.2.3-alpha"));
            Trace.Assert(Version.IsCompatible("1.2.3-alpha1"));
            Trace.Assert(Version.IsCompatible("1.2.3-alpha.1"));
            Trace.Assert(Version.IsCompatible("1.2.3-alpha.00001"));
            Trace.Assert(Version.IsCompatible("1.2.3-alpha.55667"));
            Trace.Assert(Version.IsCompatible("1.2.3-alpha.55667.131220T150332"));
        }

        public static void StableFromPrereleaseSplit() {
            throw new NotImplementedException();
        }
    }
}