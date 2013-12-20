
using System;

namespace ContinuousPackageVersioning {

    /// <summary>
    /// Expose the core API of the Continuous Package Versioning algorithm.
    /// </summary>
    public sealed class Version {
        /// <summary>
        /// The stable part of the CPV/SemVer version.
        /// </summary>
        public string Stable { get; private set; }
        /// <summary>
        /// The prerelease part of the CPV/SemVer version.
        /// </summary>
        public string Prerelease { get; private set; }
        /// <summary>
        /// The CPV version.
        /// </summary>
        public string CPVVersion { get; private set; }

        private Version(string stable, string prerelease, string cpvVersion) {
            Stable = stable;
            Prerelease = prerelease;
            CPVVersion = cpvVersion;
        }

        /// <summary>
        /// Gets the next version of a CPV-compatible version string.
        /// </summary>
        /// <param name="specified">The version as specified in its
        /// 'naked' form, e.g. "1.2.3-alpha".</param>
        /// <param name="current">The current version, if such exist.</param>
        /// <returns>The CPV version following the current one.</returns>
        public static Version GetNext(string specified, string current) {
            if (string.IsNullOrWhiteSpace(specified) && string.IsNullOrWhiteSpace(current)) {
                throw new ArgumentNullException("specified", "Specify at least either 'version' or 'current'.");
            } else if (string.IsNullOrWhiteSpace(current)) {
                // Just validate specified looks right and apply the
                // starting number to it.
                // TODO:
            }

            // Do our thing
            // TODO:
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Gets a result indicating if the given <paramref name="version"/>
        /// is a legal CPV version string.
        /// </summary>
        /// <param name="version">The version to evaluate.</param>
        /// <returns><c>true</c> if compatible; <c>false</c> otherwise.</returns>
        public static bool IsCompatible(string version) {
            string ignored1, ignored2;
            // Poor implementation; swich to non-exception-based eventually
            try {
                SplitStableFromPrerelease(version, out ignored1, out ignored2);
            } catch {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Splits a CPV-compatible string into its stable and it's
        /// prerelease part.
        /// </summary>
        /// <param name="version">The version to split.</param>
        /// <param name="stable">The stable version.</param>
        /// <param name="prerelease">The prerelease version.</param>
        /// <remarks>
        /// The hyphen in between will not be part of the results.
        /// </remarks>
        public static void SplitStableFromPrerelease(string version, out string stable, out string prerelease) {
            if (string.IsNullOrWhiteSpace(version)) {
                throw new ArgumentNullException("version");
            } else if (version.Contains("+")) {
                throw new ArgumentOutOfRangeException("Build numbers are not supported.");
            }

            var tokens = version.Split('-');
            if (tokens.Length != 2 || 
                string.IsNullOrWhiteSpace(tokens[0]) || 
                string.IsNullOrWhiteSpace(tokens[1]) ||
                !IsValidStableSemVerVersion(tokens[0])) {
                throw new ArgumentOutOfRangeException("SemVer-compatible, prerelease version string expected.");
            }

            stable = tokens[0];
            prerelease = tokens[1];
        }

        /// <inheritdoc/>
        public override string ToString() {
            return Stable + "-" + Prerelease + "." + CPVVersion;
        }

        static bool IsValidStableSemVerVersion(string version) {
            // Eventually, do this verification too.
            // TODO:
            return true;
        }

        static bool IsValidPrereleaseVersion(string version) {
            // Eventually, do this verification too.
            // TODO:
            return true;
        }
    }
}