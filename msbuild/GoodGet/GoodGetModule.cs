
using System;

namespace Modules {
    /// <summary>
    /// Represents the GoodGet module itself, and the pattern we
    /// use to support lightweight depedency injection.
    /// </summary>
    public static class GoodGetModule {

        /// <summary>
        /// Contains all dependency injections into this module
        /// </summary>
        public static class Injections {
            /// <summary>
            /// Sample dependency.
            /// </summary>
            public static IDisposable Sample = null;
        }
    }
}