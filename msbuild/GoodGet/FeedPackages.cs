
using System;
using System.Collections.Generic;

namespace GoodGet {

    /// <summary>
    /// Set of packages specified for certain <see cref="Feed"/>.
    /// </summary>
    internal sealed class FeedPackages {
        /// <summary>
        /// The feed the specified set of packages are to be
        /// synchronized with.
        /// </summary>
        public readonly Feed Feed;
        
        /// <summary>
        /// The set of packages to be synchronized from the
        /// given feed.
        /// </summary>
        public List<string> Packages = new List<string>();

        /// <summary>
        /// Initialize a <see cref="FeedPackages"/> instance.
        /// </summary>
        /// <param name="feed">The <see cref="Feed"/> to specify
        /// packages for.</param>
        /// <param name="packages">Optional packages that can be
        /// given to initialize the set of packages from the
        /// specified feed.</param>
        public FeedPackages(Feed feed, params string[] packages) {
            if (feed == null) {
                throw new ArgumentNullException("feed");
            }
            Feed = feed;
            if (packages != null) {
                foreach (var package in packages) {
                    Packages.Add(package);
                }
            }
        }
    }
}