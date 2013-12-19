
using System;
using GoodGet;

namespace Modules {
    
    /// <summary>
    /// Represents the GoodGet module itself, and the pattern we
    /// use to support lightweight depedency injection.
    /// </summary>
    public static class GoodGetModule {
        static GoodGetModule() {
            MakeDefaultDependencyInjections();
        }

        /// <summary>
        /// Contains all dependency injections into this module
        /// </summary>
        public static class Injections {
            static Injections() {
                GoodGetModule.Touch();
            }

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

        /// <summary>
        /// Supports being "touched", making sure all dependency injections
        /// are properly set before use.
        /// </summary>
        public static void Touch() {
            // Static constructor does the work.
        }

        static void MakeDefaultDependencyInjections() {
            // Pick IConsole implementation
            var console = Injections.Console;
            Injections.Console = console ?? new StandardConsole();

            // The IInstallerFactory
            var installerFactory = Injections.InstallerFactory;
            Injections.InstallerFactory = installerFactory ?? new NuGetCLIInstaller.Factory(); /*new FlowDiagnosticInstaller.Factory();*/

            // The IRestClientFactory
            var restClientFactory = Injections.RestClientFactory;
            Injections.RestClientFactory = restClientFactory ?? new NetWebClientRestClient.Factory();
        }
    }
}