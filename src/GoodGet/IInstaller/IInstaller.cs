
namespace GoodGet {
    /// <summary>
    /// Expose functionality that knows how to install a set of packages
    /// into a given packages folder.
    /// </summary>
    public interface IInstaller {
        /// <summary>
        /// Gets the <see cref="Feed"/> where the current installer
        /// fetch packages from.
        /// </summary>
        Feed Feed { get; }
 
        /// <summary>
        /// Resolves the latest version of every package part of
        /// the given <see cref="Package"/> array, based on the feed
        /// this installer work against.
        /// </summary>
        /// <param name="packages">Set of packages to evaluate.</param>
        /// <returns>Return <c>count</c> if any package was in fact
        /// out-of-date; zero otherwise. For packages that are in need
        /// of updating, the Version property should be changed either
        /// to the latest version available, if supported by the current
        /// installer, or <c>null</c> to indicate it needs to be updated,
        /// but the exact version can't be determined by this method.
        /// The count returned indicates how many packages are considered
        /// outdated.
        /// </returns>
        int QueryLatest(Package[] packages);

        /// <summary>
        /// Installs the given <see cref="Package"/> into the given
        /// <see cref="PackagesFolder"/>.
        /// </summary>
        /// <param name="folder">The folder to install packages into.</param>
        /// <param name="package">The package to install.</param>
        /// <returns>Information about the version that was installed.
        /// </returns>
        Package Install(PackagesFolder folder, Package package);
    }
}