
namespace GoodGet {
    /// <summary>
    /// Factory that knows how to construct <see cref="IInstaller"/>
    /// instances.
    /// </summary>
    public interface IInstallerFactory {
        /// <summary>
        /// Creates a new installer.
        /// </summary>
        /// <returns>An instance of an <see cref="IInstaller"/>.</returns>
        IInstaller CreateInstaller();
    }
}