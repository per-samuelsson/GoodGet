
using System;
using GoodGet;

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
            /// Gets the implementation of the <see cref="IConsole"/>
            /// that GoodGet will use when writing output.
            /// </summary>
            public static IConsole Console;

            /// <summary>
            /// A factory that knows how to construct <see cref="IInstaller"/>
            /// instances.
            /// </summary>
            public static IInstallerFactory InstallerFactory;

            /// <summary>
            /// A factory that knows how to construct <see cref="IRestClient"/>
            /// instances.
            /// </summary>
            public static IRestClientFactory RestClientFactory;
        }
    }
}