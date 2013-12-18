
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
        /// Gets the <see cref="UpdateAuthority"/> accociated with the
        /// current installer.
        /// </summary>
        IUpdateAuthority UpdateAuthority { get; }
 
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