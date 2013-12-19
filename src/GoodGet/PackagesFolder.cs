
using Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodGet {

    /// <summary>
    /// Represents a folder where a set of NuGet packages are to
    /// be kept in their latest version on any new update.
    /// </summary>
    public sealed class PackagesFolder {
        static PackagesFolder() {
            // If not before, assure we install all default
            // dependencies when this class starts being used.
            GoodGetModule.Touch();
        }

        internal Dictionary<Feed, FeedPackages> packagesPerFeed = new Dictionary<Feed, FeedPackages>();

        /// <summary>
        /// Path to where packages are installed as given when
        /// the current <see cref="PackagesFolder"/> was created.
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// The full path to where packages are installed.
        /// </summary>
        public readonly string FullPath;

        /// <summary>
        /// The set of feeds possible to download and keep packages
        /// up to date from. Keyed on their Uri.
        /// </summary>
        public readonly Dictionary<string, Feed> Feeds = new Dictionary<string, Feed>();

        /// <summary>
        /// Expose a one-line usage interface to the GoodGet module core
        /// functionality.
        /// </summary>
        /// <param name="path">The path to where packages are to be installed.</param>
        /// <param name="packages">The set of packages to install.</param>
        public static void InstallPackages(string path, params string[] packages) {
            if (packages == null || packages.Length == 0) {
                throw new ArgumentNullException("packages");
            }

            var f = new PackagesFolder(path);
            foreach (var package in packages) {
                f.AddPackage(package);
            }

            f.Install();
        }

        /// <summary>
        /// Initialize a <see cref="PackagesFolder"/> instance.
        /// </summary>
        /// <param name="path">The path to where packages are to be installed.</param>
        public PackagesFolder(string path) {
            if (string.IsNullOrWhiteSpace(path)) {
                throw new ArgumentNullException("path");
            }
            Feeds.Add(Feed.NuGetOfficial.Uri, Feed.NuGetOfficial);
            Path = path;
            FullPath = System.IO.Path.GetFullPath(path);
        }

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
        /// Installs or updates all packages registered with the current
        /// packages folder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Performs the actual core service of GoodGet: assures that
        /// all packages specified are part of the current local
        /// packages folder.
        /// </para>
        /// <para>
        /// It has been advocated that 'install' is a poor name for the
        /// NuGet CLI switch, since it does not really "install" packages
        /// into projects as expected if you come from the angle where
        /// you have used NuGet from within Visual Studio. For example,
        /// see some comments in this popular David Ebbo post:
        /// http://blog.davidebbo.com/2011/01/installing-nuget-packages-directly-from.html
        /// </para>
        /// <para>
        /// Since 'install' have been thereafter rather recognized and
        /// accepted for the CLI kind of behaviour, we'll stick to the
        /// same lingo in GoodGet too.
        /// </para>
        /// </remarks>
        public void Install() {
            var got = new GotFolder(this); /*new GotNone();*/
            RunInstall(got);
        }

        void RunInstall(IGot got) {
            var installerFactory = Modules.GoodGetModule.Injections.InstallerFactory;
            
            if (packagesPerFeed.Count > 0) {
                
                // There is at least one package we should install.
                // Assure the packages folder before we distribute it
                // out over installers.
                Utilities.AssureDirectory(FullPath);

                // Create a new installer and a new context for every feed,
                // then tell the context to install, and wait for it to finish.
                // This could easily be done in parallell if we find merit to it.
                
                foreach (var item in packagesPerFeed) {
                    var c = new InstallerContext(this, item.Value.Packages.ToArray(), installerFactory.CreateInstaller(item.Key, null), got);
                    c.Install();
                }
            }
        }
    }
}