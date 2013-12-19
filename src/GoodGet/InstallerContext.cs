
using Modules;
using System;
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

        public void Install() {
            var updateAuthority = installer.UpdateAuthority;
            var freshInstalls = new List<Package>();
            var updateCandidates = new List<Package>();

            console.WriteLine("Installing {0} packages from {1} to {2}", packages.Length, installer.Feed.DisplayName, folder.Path);

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
            var currents = (Package[])updates.Clone();
            var outdatedPackages = updateAuthority.CheckForUpdates(updates);

            foreach (var install in freshInstalls) {
                var installed = installer.Install(folder, install);
                got.Save(installed);
            }

            if (outdatedPackages > 0) {
                for (int i = 0; i < updates.Length; i++) {
                    var update = updates[i];
                    var current = currents[i];
                    if (update.Version == null || update.Version != current.Version) {
                        var installed = installer.Install(folder, update);
                        got.Save(installed);
                    }
                }
            }
        }
    }
}
