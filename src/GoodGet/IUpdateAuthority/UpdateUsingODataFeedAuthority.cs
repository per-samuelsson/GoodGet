
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

namespace GoodGet {
    /// <summary>
    /// Implements <see cref="IUpdateAuthority"/>, checking for new
    /// versions using the OData protocol.
    /// </summary>
    internal sealed class UpdateUsingODataFeedAuthority : IUpdateAuthority {
        readonly Feed feed;
        readonly IRestClient client;
        readonly TimeSpan? recencyDelay;

        /// <summary>
        /// Initialize a new <see cref="UpdateUsingODataFeedAuthority"/>
        /// instance.
        /// </summary>
        /// <param name="feed">The feed to update from.</param>
        /// <param name="restClient">The <see cref="IRestClient"/> to use.</param>
        /// <param name="recencyDelay">A delay to use that limits the check for
        /// an update to a certain recency.</param>
        public UpdateUsingODataFeedAuthority(Feed feed, IRestClient restClient, TimeSpan? recencyDelay = null) {
            this.feed = feed;
            this.client = restClient;
            this.recencyDelay = recencyDelay;
        }

        /// <inheritdoc/>
        Feed IUpdateAuthority.Feed {
            get { return feed; }
        }

        /// <inheritdoc/>
        int IUpdateAuthority.CheckForUpdates(Package[] packages) {
            if (recencyDelay != null) {
                throw new NotImplementedException("recencyDelay");
            }

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
            //     3.3) Don't get JSON - faster with other data?
            //     3.4) Use 'select' feature to only fetch the actual
            //          "IsAbsoluteLatestVersion" property.
            //   4) Check each of (3) with different number of packages in
            //      the feed - does it matter if its 5 or 500?
            //   5) Check if we can do things in parallel (using PTL).

            int count = 0;
            foreach (var p in packages) {
                var uri = string.Format("{0}(Id='{1}',Version='{2}')?$select=IsAbsoluteLatestVersion", feed.PackagesUri, p.Id, p.Version);
                var content = client.GetJSONString(uri);

                var isLatest = ParseIsLatestUsingJSONWithOnlyDotNet(content);
                if (!isLatest) {
                    count++;
                    p.Version = null;
                }
            }

            return count;
        }

        static bool ParseIsLatestUsingJSONWithOnlyDotNet(string json) {
            // Simplest possible to start with, using just the
            // bits we've got in the .NET framework.
            var x = new JavaScriptSerializer();
            var t = x.Deserialize<Dictionary<string, object>>(json);
            var d = t["d"] as IDictionary<string, object>;
            return (bool) d["IsAbsoluteLatestVersion"];
        }
    }
}