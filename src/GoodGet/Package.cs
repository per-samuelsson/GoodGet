
using System;

namespace GoodGet {

    /// <summary>
    /// Represents a NuGet package GoodGet got.
    /// </summary>
    public sealed class Package {
        /// <summary>
        /// The package identity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The package version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The time this package was last installed/updated.
        /// </summary>
        public DateTime? Installed { get; set; }
    }
}