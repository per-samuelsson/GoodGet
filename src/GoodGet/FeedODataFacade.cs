using System;

namespace GoodGet {
    /// <summary>
    /// Expose a simple facade of the functionality we need from
    /// feeds, accessing the underlying remote data using the OData
    /// protocol.
    /// </summary>
    public sealed class FeedODataFacade {
        readonly IRestClient client;

        /// <summary>
        /// Initialize a new <see cref="FeedODataFacade"/> instance.
        /// </summary>
        /// <param name="client">
        /// The REST client to use when accessing remote data.</param>
        public FeedODataFacade(IRestClient client) {
            this.client = client;
        }

        /// <summary>
        /// Gets the <see cref="Feed"/> this facade access data from.
        /// </summary>
        public Feed Feed {
            get { return client.Feed; }
        }

        /// <summary>
        /// Gets a value indicating if the given version is the latest
        /// version of the specified package.
        /// </summary>
        /// <param name="packageId">Id of the package to lookup.</param>
        /// <param name="version">The version to consult.</param>
        /// <returns><c>true</c> if the given version is in fact the
        /// latest version, based on the underlying OData; <c>false</c>
        /// otherwise.</returns>
        public bool IsLatestVersion(string packageId, string version) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the latest version of a package, based on the data
        /// retreived from the underlying OData service.
        /// </summary>
        /// <param name="packageId">Id of the package whose latest
        /// version are to be looked up.</param>
        /// <returns>The latest version of said package, or <c>null</c>
        /// if the package does not exist or reveal such information.
        /// </returns>
        public string GetLatestVersion(string packageId) {
            throw new NotImplementedException();
        }
    }
}