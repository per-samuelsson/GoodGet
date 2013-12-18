
using System;

namespace GoodGet {
    /// <summary>
    /// Implements <see cref="IUpdateAuthority"/> and instruct clients to
    /// update after a certain time span has elapsed since a package was
    /// last installed.
    /// </summary>
    internal sealed class UpdateOnIntervalAuthority : IUpdateAuthority {
        readonly Feed feed;
        readonly TimeSpan interval;

        /// <summary>
        /// Initialize a new <see cref="UpdateOnIntervalAuthority"/>
        /// instance.
        /// </summary>
        /// <param name="feed">The feed to update from.</param>
        /// <param name="interval">The time between every update.</param>
        public UpdateOnIntervalAuthority(Feed feed, TimeSpan interval) {
            if (interval == null) {
                throw new ArgumentNullException("interval");
            }

            this.feed = feed;
            this.interval = interval;
        }

        /// <inheritdoc/>
        Feed IUpdateAuthority.Feed {
            get { return feed; }
        }

        /// <inheritdoc/>
        int IUpdateAuthority.CheckForUpdates(Package[] packages) {
            int count = 0;
            foreach (var p in packages) {
                var update = p.Installed == null;
                update |= DateTime.Now.Subtract(p.Installed.Value) > interval;
                if (update) {
                    count++;
                    p.Version = null;
                }
                
            }
            return count;
        }
    }
}
