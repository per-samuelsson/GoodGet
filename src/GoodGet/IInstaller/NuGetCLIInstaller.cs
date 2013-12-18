
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace GoodGet {

    internal sealed class NuGetCLIInstaller : IInstaller {
        readonly Feed feed;
        readonly IUpdateAuthority updateAuthority;
        readonly bool downloadExeIfNotFound = false;

        public sealed class Factory : IInstallerFactory {
            IInstaller IInstallerFactory.CreateInstaller(Feed feed, IUpdateAuthority updateAuthority) {
                updateAuthority = updateAuthority ?? new UpdateUsingODataFeedAuthority(feed);
                return new NuGetCLIInstaller(feed, updateAuthority);
            }
        }

        private NuGetCLIInstaller(Feed feed, IUpdateAuthority updateAuthority) {
            this.feed = feed;
            this.updateAuthority = updateAuthority;
        }

        Feed IInstaller.Feed {
            get { return feed; }
        }

        IUpdateAuthority IInstaller.UpdateAuthority {
            get { return updateAuthority; }
        }

        Package IInstaller.Install(PackagesFolder folder, Package package) {
            if (downloadExeIfNotFound) {
                throw new NotSupportedException();
            }

            var targetDir = Path.Combine(folder.FullPath, package.Id);
            if (Directory.Exists(targetDir)) {
                Directory.Delete(targetDir, true);
            }

            // Use nuget.exe 'install' to install. Update the package
            // reference returned with the version we in fact installed.
            
            var start = new ProcessStartInfo();
            start.FileName = "nuget.exe";
            start.Arguments = string.Format(
                "install {0} -Source {1} -OutputDirectory {2} -Prerelease -NonInteractive", 
                package.Id,
                feed.Uri,
                folder.FullPath
            );
            try {
                var output = new List<string>();
                var result = Utilities.ToolProcess.Invoke(start, output, output);
                if (result != 0) {
                    foreach (var s in output) {
                        Console.WriteLine(s);
                    }
                    throw new Exception(string.Format("nuget.exe failed with code {0}", result));
                }
            } catch (Win32Exception e) {
                if (e.NativeErrorCode == 2) {
                    if (downloadExeIfNotFound) {
                        // If nuget.exe is not accessible (e.g. not in the PATH or in
                        // the local folder), we could offer downloading it on demand.
                        // We could also offer a NuGet path as a configuration alternative
                        // when any NuGet CLI components are in effect.
                        
                        // Download NuGet.exe.
                        // TODO:
                    }
                }
                throw;
            }

            // Locate the installed package by the package name and extract the
            // version from that.

            var installationDir = FindInstalledDirectoryWithVersion(folder.FullPath, package.Id);
            var version = installationDir.Name.Substring(package.Id.Length + 1);

            installationDir.MoveTo(Path.Combine(folder.FullPath, package.Id));
            return new Package() { Id = package.Id, Version = version, Installed = DateTime.Now };
        }

        static DirectoryInfo FindInstalledDirectoryWithVersion(string packagesFolder, string package) {
            // Consider:
            //   Foo
            //     Bar.1.2.3
            //     Bar.Core.1.2.3
            //
            // If we pass ("Foo", "Bar"), we can't let "Bar.Core" be considered.

            // This can probably be optimized quite a lot. Not sure what the
            // penalty is though, so we need to check if optimizing has merit.

            var dirs = Directory.GetDirectories(packagesFolder);
            DirectoryInfo result = null;
            foreach (var folder in dirs) {
                var info = new DirectoryInfo(folder);
                if (info.Name.StartsWith(package, StringComparison.InvariantCultureIgnoreCase)) {
                    if (result == null) {
                        result = info;
                    } else {
                        result = result.CreationTime > info.CreationTime ? result : info;
                    }
                }
            }
            return result;
        }
    }
}
