
namespace GoodGet {
    /// <summary>
    /// Expose functionality that knows how to install a set of packages
    /// into a given packages folder.
    /// </summary>
    public interface IInstaller {
        /// <summary>
        /// Installs all given packages into <see cref="PackagesFolder"/>.
        /// </summary>
        /// <param name="folder">The folder to install packages into.</param>
        /// <param name="packages">The packages to install.</param>
        /// <param name="got">The authourity to ask when the installer need
        /// to know what packages are installed.</param>
        void Install(PackagesFolder folder, FeedPackages packages, IGot got);
    }
}