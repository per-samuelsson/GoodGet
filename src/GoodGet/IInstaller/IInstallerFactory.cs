﻿
namespace GoodGet {
    /// <summary>
    /// Factory that knows how to construct <see cref="IInstaller"/>
    /// instances.
    /// </summary>
    public interface IInstallerFactory {
        /// <summary>
        /// Creates a new installer.
        /// </summary>
        /// <param name="feed">The feed to create the installer for.</param>
        /// <param name="updateAuthority">A reference to a component implementing
        /// the update authority to bind to the installer.</param>
        /// <returns>An instance of an <see cref="IInstaller"/>.</returns>
        IInstaller CreateInstaller(Feed feed, IUpdateAuthority updateAuthority);
    }
}