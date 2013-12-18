
using System;

namespace GoodGet {
    /// <summary>
    /// Implements <see cref="IUpdateAuthority"/>, checking for new
    /// versions using the OData protocol.
    /// </summary>
    internal sealed class UpdateUsingODataFeedAuthority : IUpdateAuthority {
        readonly Feed feed;
        readonly TimeSpan? recencyDelay;

        /// <summary>
        /// Initialize a new <see cref="UpdateUsingODataFeedAuthority"/>
        /// instance.
        /// </summary>
        /// <param name="feed">The feed to update from.</param>
        /// <param name="recencyDelay">A delay to use that limits the check for
        /// an update to a certain recency.</param>
        public UpdateUsingODataFeedAuthority(Feed feed, TimeSpan? recencyDelay = null) {
            this.feed = feed;
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
            //     3.3) More?
            //   4) Check each of (3) with different number of packages in
            //      the feed - does it matter if its 5 or 500?
            //   5) Check if we can do things in parallel (using PTL).
            // TODO:

            throw new NotImplementedException();
        }
    }
}