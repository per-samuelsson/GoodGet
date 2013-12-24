
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ContinuousPackageVersioning {

    /// <summary>
    /// Expose the core API of the Continuous Package Versioning algorithm.
    /// </summary>
    public sealed class Version {
        const string startingVersion = "00000";
        const int highestCpvNumber = 99999;
        static Regex cpvPrereleaseRegex = new Regex(@"\.(\d{5})\z");

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
        /// Converts the CPV-based <paramref name="version"/> string to
        /// its corresponding <see cref="Version"/> instance.
        /// </summary>
        /// <param name="version">The version string to parse.</param>
        /// <returns>A <see cref="Version"/> instance.</returns>
        public static Version Parse(string version) {
            string stable, prerelease, cpv;
            SplitStableFromPrerelease(version, out stable, out prerelease);
            SplitCPVFromPrerelease(prerelease, out prerelease, out cpv);
            return new Version(stable, prerelease, cpv);
        }

        /// <summary>
        /// Gets the next version of a CPV-compatible version string.
        /// </summary>
        /// <param name="specified">The version as specified in its
        /// 'naked' form, e.g. "1.2.3-alpha".</param>
        /// <param name="current">The current version, if such exist.</param>
        /// <returns>The CPV version following the current one.</returns>
        public static Version GetNext(string specified, string current = null) {
            string stable, prerelease, cpv;
            if (string.IsNullOrWhiteSpace(specified) && string.IsNullOrWhiteSpace(current)) {
                throw new ArgumentNullException("specified", "Specify at least either 'version' or 'current'.");
            } else if (string.IsNullOrWhiteSpace(current)) {
                // Just validate specified looks right and apply the
                // starting number to it.
                SplitStableFromPrerelease(specified, out stable, out prerelease);
                return new Version(stable, prerelease, startingVersion);

            } else if (!string.IsNullOrWhiteSpace(specified)) {
                // Both the specified and the current one are specified.
                // We should check them, to see if we need to restart.
                if (!current.StartsWith(specified)) {
                    SplitStableFromPrerelease(specified, out stable, out prerelease);
                    return new Version(stable, prerelease, startingVersion);
                }
            }

            SplitStableFromPrerelease(current, out stable, out prerelease);
            SplitCPVFromPrerelease(prerelease, out prerelease, out cpv);
            cpv = ParseAndIncreaseCPVVersionString(cpv);
            return new Version(stable, prerelease, cpv);
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
            // Implement this eventually.
            // TODO:
            return true;
        }

        static bool IsValidPrereleaseVersion(string prerelease) {
            try {
                string a, b;
                SplitCPVFromPrerelease(prerelease, out a, out b);
            } catch {
                return false;
            }
            return true;
        }

        static void SplitCPVFromPrerelease(string prereleaseWithCPV, out string prerelease, out string cpv) {
            var tokens = cpvPrereleaseRegex.Split(prereleaseWithCPV);
            if (tokens.Length != 3 || tokens[2] != string.Empty) {
                throw new ArgumentOutOfRangeException("Prerelease part is not CPV compatible: {0}", prereleaseWithCPV);
            }
            prerelease = tokens[0];
            cpv = tokens[1];
        }

        static string ParseAndIncreaseCPVVersionString(string cpvString) {
            var cpv = int.Parse(cpvString);
            if (++cpv > highestCpvNumber) {
                // Series exhausted.
                // TODO:
                throw new ArithmeticException();
            }
            return string.Format("{0:D5}", cpv);
        }
    }
}