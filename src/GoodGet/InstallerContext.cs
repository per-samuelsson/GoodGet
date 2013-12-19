
using Modules;
using System.Collections.Generic;

namespace GoodGet {
    /// <summary>
    /// Container for a given <see cref="IInstaller"/>, feeding the installer
    /// with the neccessary input and controlling the order in which packages
    /// are installed and/or updated.
    /// </summary>
    internal sealed class InstallerContext {
        readonly IConsole console = GoodGetModule.Injections.Console;
        readonly PackagesFolder folder;
        readonly string[] packages;
        readonly IInstaller installer;
        readonly IGot got;

        public InstallerContext(PackagesFolder folder, string[] packages, IInstaller installer, IGot got) {
            this.folder = folder;
            this.packages = packages;
            this.installer = installer;
            this.got = got;
        }

        public bool Install() {
            var updateAuthority = installer.UpdateAuthority;
            var freshInstalls = new List<Package>();
            var updateCandidates = new List<Package>();

            console.WriteLine(
                Rank.Debug, 
                "Begin checking {0} packages from \"{1}\" against {2}", packages.Length, installer.Feed.DisplayName, folder.Path
                );

            var installedPackages = got.Get(packages);

            // Build a list of all packages that either need to install from scratch,
            // or those that need to be checked.

            for (int i = 0; i < installedPackages.Length; i++) {
                var installed = installedPackages[i];
                if (installed == null) {
                    freshInstalls.Add(new Package() { Id = packages[i]});
                } else {
                    updateCandidates.Add(installed);
                }
            }

            // Install all the new packages in paralell to querying the
            // outdated ones. Install eventual updates last.
            // Note: Do it serialized in this first version.

            var updates = updateCandidates.ToArray();
            var currents = updates.Clone<Package>();
            var outdatedPackages = updateAuthority.CheckForUpdates(updates);

            if (freshInstalls.Count > 0) {
                console.WriteLine(
                    Rank.Debug,
                    "Begin installing {0} packages from \"{1}\" into {2}...", freshInstalls.Count, installer.Feed.DisplayName, folder.Path
                );

                foreach (var install in freshInstalls) {
                    console.WriteLine("Installing {0} from \"{1}\" into {2}...", install.Id, installer.Feed.DisplayName, folder.Path);
                    var installed = installer.Install(folder, install);
                    got.Save(installed);
                    console.WriteLine("Done (Version {0} installed)", installed.Version);
                }

                console.WriteLine(
                    Rank.Debug,
                    "Done installing {0} packages from \"{1}\" into {2}", freshInstalls.Count, installer.Feed.DisplayName, folder.Path
                );
            }

            if (outdatedPackages > 0) {
                console.WriteLine(
                    Rank.Debug,
                    "Begin updating {0} packages from \"{1}\" in {2}", freshInstalls.Count, installer.Feed.DisplayName, folder.Path
                );

                for (int i = 0; i < updates.Length; i++) {
                    var update = updates[i];
                    var current = currents[i];
                    if (update.Version == null || update.Version != current.Version) {
                        console.WriteLine("Updating {0} from \"{1}\" in {2}...", current.Id, installer.Feed.DisplayName, folder.Path);
                        var installed = installer.Install(folder, update);
                        got.Save(installed);
                        console.WriteLine("Done ({0} -> {1})", current.Version, installed.Version);
                    }
                }

                console.WriteLine(
                    Rank.Debug,
                    "Done updating {0} packages from \"{1}\" in {2}", freshInstalls.Count, installer.Feed.DisplayName, folder.Path
                );
            }

            console.WriteLine(
                Rank.Debug, 
                "Finished checking {0} packages from \"{1}\" targetting {2}; {3} updated, {4} installed", 
                packages.Length, 
                installer.Feed.DisplayName, 
                folder.Path,
                outdatedPackages,
                freshInstalls.Count
                );

            return outdatedPackages > 0 || freshInstalls.Count > 0;
        }
    }
}
