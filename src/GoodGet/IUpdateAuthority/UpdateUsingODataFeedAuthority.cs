
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
        readonly FeedODataFacade odataFeed;
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
            odataFeed = new FeedODataFacade(restClient);
            this.recencyDelay = recencyDelay;
        }

        /// <inheritdoc/>
        Feed IUpdateAuthority.Feed {
            get { return odataFeed.Feed; }
        }

        /// <inheritdoc/>
        int IUpdateAuthority.CheckForUpdates(Package[] packages) {
            if (recencyDelay != null) {
                throw new NotImplementedException("recencyDelay");
            }

            int count = 0;
            foreach (var p in packages) {
                var isLatest = odataFeed.IsLatestVersion(p.Id, p.Version);
                if (!isLatest) {
                    count++;
                    p.Version = null;
                }
            }

            return count;
        }
    }
}