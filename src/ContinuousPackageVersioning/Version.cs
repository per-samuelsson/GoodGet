
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ContinuousPackageVersioning {

    /// <summary>
    /// Expose the core API of the Continuous Package Versioning algorithm.
    /// </summary>
    public sealed class Version {
        const string startingSequence = "00000";
        const int maxSequence = 99999;
        static Regex sequenceRegex = new Regex(@"\.(\d{5})\z");

        /// <summary>
        /// The stable part of the CPV/SemVer version.
        /// </summary>
        public string Stable { get; private set; }

        /// <summary>
        /// The prerelease part of the CPV/SemVer version.
        /// </summary>
        public string Prerelease { get; private set; }

        /// <summary>
        /// The CPV version sequence number.
        /// </summary>
        public string Sequence { get; private set; }

        /// <summary>
        /// Initialize a <see cref="Version"/> based on a given
        /// static version, i.e. a version that includes the
        /// required stable- and prerelease parts of a CPV-version,
        /// but that excludes the sequence.
        /// </summary>
        /// <param name="staticVersion">The static version.</param>
        public Version(string staticVersion) {
            string stable, pre;
            SplitStableFromPrerelease(staticVersion, out stable, out pre);
            this.Stable = stable;
            this.Prerelease = pre;
            this.Sequence = startingSequence;
        }

        private Version(string stable, string prerelease, string sequence) {
            Stable = stable;
            Prerelease = prerelease;
            Sequence = sequence;
        }

        /// <summary>
        /// Converts the CPV-based <paramref name="version"/> string to
        /// its corresponding <see cref="Version"/> instance.
        /// </summary>
        /// <param name="version">The version string to parse.</param>
        /// <returns>A <see cref="Version"/> instance.</returns>
        public static Version Parse(string version) {
            string stable, prerelease, seq;
            SplitStableFromPrerelease(version, out stable, out prerelease);
            SplitSequenceFromPrerelease(prerelease, out prerelease, out seq);
            return new Version(stable, prerelease, seq);
        }

        /// <summary>
        /// Increase the current versions CPV sequence with one, and
        /// returns a new version representing the increase.
        /// </summary>
        /// <returns>A new version with a CPV sequence increasing that
        /// of the current instance with 1.</returns>
        public Version Next() {
            return new Version(this.Stable, this.Prerelease, ParseAndIncreaseSequence(this.Sequence));
        }

        /// <summary>
        /// Gets the next version of a CPV-compatible version string.
        /// </summary>
        /// <param name="specified">The version as specified in its
        /// static form, e.g. "1.2.3-alpha".</param>
        /// <param name="current">The current version, if such exist.</param>
        /// <returns>The CPV version following the current one.</returns>
        public static Version GetNext(string specified, string current = null) {
            string stable, prerelease, seq;
            if (string.IsNullOrWhiteSpace(specified) && string.IsNullOrWhiteSpace(current)) {
                throw new ArgumentNullException("specified", "Specify at least either 'version' or 'current'.");
            } else if (string.IsNullOrWhiteSpace(current)) {
                // Just validate specified looks right and apply the
                // starting number to it.
                SplitStableFromPrerelease(specified, out stable, out prerelease);
                return new Version(stable, prerelease, startingSequence);

            } else if (!string.IsNullOrWhiteSpace(specified)) {
                // Both the specified and the current one are specified.
                // We should check them, to see if we need to restart.
                if (!current.StartsWith(specified)) {
                    SplitStableFromPrerelease(specified, out stable, out prerelease);
                    return new Version(stable, prerelease, startingSequence);
                }
            }

            SplitStableFromPrerelease(current, out stable, out prerelease);
            SplitSequenceFromPrerelease(prerelease, out prerelease, out seq);
            seq = ParseAndIncreaseSequence(seq);
            return new Version(stable, prerelease, seq);
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
            return Stable + "-" + Prerelease + "." + Sequence;
        }

        static bool IsValidStableSemVerVersion(string version) {
            // Implement this eventually.
            // TODO:
            return true;
        }

        static bool IsValidPrereleaseVersion(string prerelease) {
            try {
                string a, b;
                SplitSequenceFromPrerelease(prerelease, out a, out b);
            } catch {
                return false;
            }
            return true;
        }

        static void SplitSequenceFromPrerelease(string prereleaseWithCPV, out string prerelease, out string seq) {
            var tokens = sequenceRegex.Split(prereleaseWithCPV);
            if (tokens.Length != 3 || tokens[2] != string.Empty) {
                throw new ArgumentOutOfRangeException("Prerelease part is not CPV compatible: {0}", prereleaseWithCPV);
            }
            prerelease = tokens[0];
            seq = tokens[1];
        }

        static string ParseAndIncreaseSequence(string sequence) {
            var seq = int.Parse(sequence);
            if (++seq > maxSequence) {
                // Series exhausted.
                // TODO:
                throw new ArgumentOutOfRangeException(
                    string.Format("The sequence is exhaused. The maximum sequence for a given version is {0}", maxSequence));
            }
            return string.Format("{0:D5}", seq);
        }
    }
}