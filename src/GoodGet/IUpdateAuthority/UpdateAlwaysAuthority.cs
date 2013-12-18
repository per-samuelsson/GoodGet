
namespace GoodGet {

    /// <summary>
    /// Implements <see cref="IUpdateAuthority"/>, instructing callers
    /// to update to a latest, unknown version on every call.
    /// </summary>
    internal sealed class UpdateAlwaysAuthority : IUpdateAuthority {
        readonly Feed feed;

        /// <summary>
        /// Initialize a new <see cref="UpdateAlwaysAuthority"/> instance.
        /// </summary>
        /// <param name="feed">The feed to update from.</param>
        public UpdateAlwaysAuthority(Feed feed) {
            this.feed = feed;
        }

        /// <inheritdoc/>
        Feed IUpdateAuthority.Feed {
            get { return feed; }
        }

        /// <inheritdoc/>
        int IUpdateAuthority.CheckForUpdates(Package[] packages) {
            foreach (var package in packages) {
                package.Version = null;
            }
            return packages.Length;
        }
    }
}