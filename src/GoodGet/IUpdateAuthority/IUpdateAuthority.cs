
namespace GoodGet {
    /// <summary>
    /// Expose the functionality of the component that governs what
    /// package are considered outdated.
    /// </summary>
    public interface IUpdateAuthority {
        /// <summary>
        /// Gets the <see cref="Feed"/> where the current authority
        /// are to consult for newer versions.
        /// </summary>
        Feed Feed { get; }

        /// <summary>
        /// Resolves the latest version of every package part of
        /// the given <see cref="Package"/> array, based on the feed
        /// this authority work against.
        /// </summary>
        /// <param name="packages">Set of packages to evaluate.</param>
        /// <returns>
        /// Return a <c>count</c> that indicates how many packages are
        /// outdated. Zero means all packages are considered up-to-date.
        /// </returns>
        /// <remarks>
        /// Return a <c>count</c> that indicates how many packages are
        /// outdated. Zero means all packages are considered up-to-date.
        /// For packages that are in need of updating, the Version property
        /// should be changed either to the latest version available, if
        /// supported by the current authority, or <c>null</c> to indicate
        /// it needs to be updated, but the exact version can't be determined
        /// by this method.
        /// </remarks>
        int CheckForUpdates(Package[] packages);
    }
}