using System;

namespace GoodGet {

    /// <summary>
    /// Implements the <see cref="IConsole"/> interface by writing
    /// content to the standard console streams (stdout, stderr).
    /// </summary>
    internal sealed class StandardConsole : IConsole {
        /// <summary>
        /// Gets or sets a value instructing the console to
        /// omit any output written.
        /// </summary>
        public bool Quiet {
            get { return CurrentSeverityLevel < 0; }
            set { CurrentSeverityLevel = -1; }
        }

        /// <summary>
        /// Gets or sets the severity level that govers what
        /// messages gets written to the underlying console.
        /// </summary>
        public int CurrentSeverityLevel { get; set; }

        /// <inheritdoc/>
        void IConsole.WriteLine(string format, params object[] args) {
            WriteLineToConsole(Rank.Info, format, args);
        }

        /// <inheritdoc/>
        void IConsole.WriteLine(int severity, string format, params object[] args) {
            WriteLineToConsole(severity, format, args);
        }

        void WriteLineToConsole(int severity, string format, params object[] args) {
            if (severity >= CurrentSeverityLevel) {
                Console.WriteLine(format, args);
                if (severity == Rank.Error) {
                    Console.Error.WriteLine(format, args);
                }
            }
        }
    }
}