
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodGet {

    /// <summary>
    /// Represents a folder where a set of NuGet packages are to
    /// be kept in their latest version on any new update.
    /// </summary>
    public sealed class PackagesFolder {
        internal Dictionary<Feed, FeedPackages> packagesPerFeed = new Dictionary<Feed, FeedPackages>();

        /// <summary>
        /// The set of feeds possible to download and keep packages
        /// up to date from. Keyed on their Uri.
        /// </summary>
        public readonly Dictionary<string, Feed> Feeds = new Dictionary<string, Feed>();

        /// <summary>
        /// Adds a package to be contained in the current packages
        /// folder, optionally specifying what feed to synchronize it
        /// with.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If no feed are given, the package is registered with the
        /// single feed of the current packages folder. If no such feed
        /// exist, or there are more than one feed, an exception is
        /// raised.
        /// </para>
        /// <para>
        /// If a feed is specified and it has not been added previously,
        /// it is added automatically.
        /// </para>
        /// <para>
        /// If a feed is specified and the given package has been added
        /// for that feed previously, this method silently succeeds.
        /// </para>
        /// </remarks>
        /// <param name="packageId">Identity of the package to include.</param>
        /// <param name="feed">The feed to synchronize the package with
        /// on future updates.</param>
        public void AddPackage(string packageId, Feed feed = null) {
            if (feed == null) {
                if (Feeds.Count == 0) {
                    throw new InvalidOperationException("No feeds added to add package too. Add at least one feed.");
                } else if (Feeds.Count > 1) {
                    throw new InvalidOperationException("Multiple feeds registered. Specify feed to add to.");
                } else {
                    feed = Feeds.First().Value;
                }
            }

            // Get the feed and the set of packages registered with it,
            // i.e. the FeedPackages for the given feed. Create both if
            // needed.

            Feed registeredFeed;
            var result = Feeds.TryGetValue(feed.Uri, out registeredFeed);
            if (!result) {
                Feeds.Add(feed.Uri, feed);
                packagesPerFeed.Add(feed, new FeedPackages(feed, packageId));
            } else {
                FeedPackages feedPackages;
                result = packagesPerFeed.TryGetValue(feed, out feedPackages);
                if (!result) {
                    packagesPerFeed.Add(feed, new FeedPackages(feed, packageId));
                } else {
                    if (!feedPackages.Packages.Contains(packageId)) {
                        feedPackages.Packages.Add(packageId);
                    }
                }
            }
        }

        /// <summary>
        /// Updates all packages registered with the current packages
        /// folder.
        /// </summary>
        /// <remarks>
        /// Performs the actual core service of GoodGet: assures that
        /// all packages specified are part of the current local
        /// packages folder.
        /// </remarks>
        public void Update() {
            throw new NotImplementedException();
        }
    }
}