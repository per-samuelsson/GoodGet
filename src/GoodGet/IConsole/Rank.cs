
namespace GoodGet {
    /// <summary>
    /// Expose a set of constants indicating the rank of importance
    /// for content being written to <see cref="IConsole"/> implementations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Not naming it "Importance" or "Severity" or similar is by design,
    /// because I feel the longer it is, the more cluttery the code.
    /// </para>
    /// <para>
    /// We use a static class with integer constants rather than an enum
    /// to allow future implementations of the <see cref="IConsole"/>
    /// interface depend on less types by force.
    /// </para>
    /// </remarks>
    public static class Rank {
        public const int Debug = 0;
        public const int Info = 1;
        public const int Notice = 2;
        public const int Warning = 3;
        public const int Error = 4;
    }
}