
using System;

namespace GoodGet {

    internal sealed class FlowDiagnosticInstaller : IInstaller {
        readonly Feed feed;

        public sealed class Factory : IInstallerFactory {
            IInstaller IInstallerFactory.CreateInstaller(Feed feed) {
                return new FlowDiagnosticInstaller(feed);
            }
        }

        public FlowDiagnosticInstaller(Feed f) {
            feed = f;
        }

        Feed IInstaller.Feed {
            get { return feed; }
        }

        int IInstaller.QueryLatest(Package[] packages) {
            Console.WriteLine("{0}:{1}: Query latest for {2} packages", GetType().Name, feed.DisplayName, packages.Length);
            foreach (var p in packages) {
                p.Version = null;
            }
            return packages.Length;
        }

        Package IInstaller.Install(PackagesFolder folder, Package package) {
            Console.WriteLine("{0}:{1}: Installing package {2} into {3}", GetType().Name, feed.DisplayName, package.Id, folder.Path);
            var result = new Package { Id = package.Id, Version = "1.2.3" };
            return result;
        }
    }
}
