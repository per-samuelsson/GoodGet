﻿
using Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace GoodGet {

    internal sealed class NuGetCLIInstaller : IInstaller {
        readonly IConsole console = GoodGetModule.Injections.Console;
        readonly Feed feed;
        readonly IUpdateAuthority updateAuthority;
        readonly bool downloadExeIfNotFound = false;

        public sealed class Factory : IInstallerFactory {
            IInstaller IInstallerFactory.CreateInstaller(Feed feed, IUpdateAuthority updateAuthority) {
                updateAuthority = updateAuthority ?? new UpdateUsingODataFeedAuthority(feed, GoodGetModule.Injections.RestClientFactory.CreateClient(feed));
                return new NuGetCLIInstaller(feed, updateAuthority);
            }
        }

        private NuGetCLIInstaller(Feed feed, IUpdateAuthority updateAuthority) {
            this.feed = feed;
            this.updateAuthority = updateAuthority;
            downloadExeIfNotFound = true;
        }

        Feed IInstaller.Feed {
            get { return feed; }
        }

        IUpdateAuthority IInstaller.UpdateAuthority {
            get { return updateAuthority; }
        }

        Package IInstaller.Install(PackagesFolder folder, Package package) {
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

            var attemptDownload = downloadExeIfNotFound;
            run_nuget:
            try {
                var output = new List<string>();
                var result = Utilities.ToolProcess.Invoke(start, output, output);
                if (result != 0) {
                    var message = string.Format("nuget.exe failed with code {0}", result);
                    var errorSeverity = 4;

                    console.WriteLine(errorSeverity, message);
                    foreach (var s in output) {
                        console.WriteLine(errorSeverity, s);
                    }

                    throw new Exception(message);
                }
            } catch (Win32Exception e) {
                if (e.NativeErrorCode == 2) {
                    if (attemptDownload) {
                        console.WriteLine(Rank.Notice, "NuGet.exe not found. Downloading it...");

                        // If nuget.exe is not accessible (e.g. not in the PATH or in
                        // the local folder), we download it on demand.
                        // We could also offer a NuGet path as a configuration alternative
                        // when any NuGet CLI components are in effect.

                        var exeDir = Path.GetDirectoryName(GetType().Assembly.Location);
                        var targetPath = Path.Combine(exeDir, "nuget.exe");

                        var client = new WebClient();
                        client.DownloadFile("https://www.nuget.org/nuget.exe", targetPath);
                        console.WriteLine(Rank.Debug, "NuGet.exe downloaded to {0}", targetPath);
                        attemptDownload = false;
                        goto run_nuget;
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
