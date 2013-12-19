
using System;

namespace GoodGet {

    /// <summary>
    /// Represents a NuGet package GoodGet got.
    /// </summary>
    public sealed class Package: ICloneable {
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

        /// <summary>
        /// Clones the current package using a memberwise copy
        /// strategy, similar to <see cref="object.MemberwiseClone"/>.
        /// </summary>
        /// <returns>A clone of the current <see cref="Package"/>.
        /// </returns>
        public new Package MemberwiseClone() {
            return new Package { Id = Id, Version = Version, Installed = Installed };
        }

        object ICloneable.Clone() {
            return MemberwiseClone();
        }
    }
}