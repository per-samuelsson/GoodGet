using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

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
        /// Gets the <see cref="IRestClient"/> this facade access use
        /// when accessing the underlying OData.
        /// </summary>
        public IRestClient Client {
            get { return client; }
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
            #region Ramblings
            // Since this is the most performance-critical spot in
            // GoodGet, we should invest most of our time trying
            // different alternatives here to make things snappy.
            // Here's a list of things:
            //   1) Different JSON-parsers / deserializers
            //   2) Different REST/HTTP clients
            //   3) Different OData approaches:
            //     3.1) Get entry based on name and the version "got", then
            //          check "IsAbsoluteLatestVersion" locally.
            //     3.2) Get entry based on name and '$filter' the feed for
            //          the one that is the latest, and compare the version
            //          we get back to "got".
            //     Examples: 
            //       https://www.myget.org/F/Starcounter/Packages?$filter=Id eq 'Starcounter.ErrorCodes' and IsAbsoluteLatestVersion
            //       https://www.myget.org/F/Starcounter/Packages?$filter=Id eq 'Starcounter.ErrorCodes' and IsAbsoluteLatestVersion&$select=Version
            //       https://www.nuget.org/api/v2/Packages?$filter=Id eq 'GoodGet' and IsAbsoluteLatestVersion
            //       https://www.nuget.org/api/v2/Packages?$filter=Id eq 'GoodGet' and IsAbsoluteLatestVersion&$select=Version
            //     3.3) Don't get JSON - faster with other data?
            //     3.4) Use 'select' feature to only fetch the actual
            //          "IsAbsoluteLatestVersion" property.
            //   4) Check each of (3) with different number of packages in
            //      the feed - does it matter if its 5 or 500?
            //   5) Check if we can do things in parallel (using PTL).
            #endregion

            var uri = string.Format("{0}(Id='{1}',Version='{2}')?$select=IsAbsoluteLatestVersion", Feed.PackagesUri, packageId, version);
            var content = client.GetJSONString(uri);

            var x = new JavaScriptSerializer();
            var t = x.Deserialize<Dictionary<string, object>>(content);
            var d = t["d"] as IDictionary<string, object>;
            return (bool)d["IsAbsoluteLatestVersion"];
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