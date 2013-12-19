
namespace GoodGet {
    /// <summary>
    /// Expose methods to write to a "console", i.e. an output
    /// stream that normally somehow reach the user.
    /// </summary>
    public interface IConsole {
        /// <summary>
        /// Writes the output using the severity level considered to
        /// be most standard to the actual implementation.
        /// </summary>
        /// <param name="format">A composite string format string.</param>
        /// <param name="args">Arguments to insert.</param>
        void WriteLine(string format, params object[] args);

        /// <summary>
        /// Writes the output using the severity level specified.
        /// </summary>
        /// <param name="severity">The severity of the written content.</param>
        /// <param name="format">A composite string format string.</param>
        /// <param name="args">Arguments to insert.</param>
        void WriteLine(int severity, string format, params object[] args);
    }
}
