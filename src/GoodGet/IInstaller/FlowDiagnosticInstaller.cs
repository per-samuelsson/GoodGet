
using System;

namespace GoodGet {

    internal sealed class FlowDiagnosticInstaller : IInstaller, IUpdateAuthority {
        readonly Feed feed;
        IUpdateAuthority updateAuthority;

        public sealed class Factory : IInstallerFactory {
            IInstaller IInstallerFactory.CreateInstaller(Feed feed, IUpdateAuthority updateAuthority) {
                return new FlowDiagnosticInstaller(feed, updateAuthority);
            }
        }

        public FlowDiagnosticInstaller(Feed f, IUpdateAuthority updateAuthority = null) {
            feed = f;
            this.updateAuthority = updateAuthority ?? this;
        }

        Feed IInstaller.Feed {
            get { return feed; }
        }

        Feed IUpdateAuthority.Feed {
            get { return feed; }
        }

        int IUpdateAuthority.CheckForUpdates(Package[] packages) {
            Console.WriteLine("{0}:{1}: Query latest for {2} packages", GetType().Name, feed.DisplayName, packages.Length);
            foreach (var p in packages) {
                p.Version = null;
            }
            return packages.Length;
        }

        IUpdateAuthority IInstaller.UpdateAuthority {
            get { return updateAuthority; }
        }

        Package IInstaller.Install(PackagesFolder folder, Package package) {
            Console.WriteLine("{0}:{1}: Installing package {2} into {3}", GetType().Name, feed.DisplayName, package.Id, folder.Path);
            var result = new Package { Id = package.Id, Version = "1.2.3" };
            return result;
        }
    }
}
