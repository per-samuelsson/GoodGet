
namespace GoodGet {

    /// <summary>
    /// Represents a NuGet feed, e.g. the official NuGet feed
    /// or a named MyGet feed.
    /// </summary>
    public sealed class Feed {
        /// <summary>
        /// Reference to a <see cref="Feed"/> that represent the official 
        /// NuGet feed.
        /// </summary>
        public static readonly Feed NuGetOfficial = new Feed() {
            Uri = "https://www.nuget.org/api/v2/",
            PackagesUri = "https://www.nuget.org/api/v2/Packages",
            DisplayName = "Official NuGet feed"
        };

        /// <summary>
        /// The Uri of the source itself, as used in standard
        /// NuGet lingo, e.g. when doing command-line nuget.exe
        /// 'install' and specifying '-source'.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The Uri to where packages live, as used in REST/Odata
        /// access to nuget-compatible feeds.
        /// </summary>
        public string PackagesUri { get; set; }

        /// <summary>
        /// The human-friendly display name for the feed.
        /// </summary>
        public string DisplayName { get; set; }
    }
}