
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

// Backlog:
//
// Feature / usability
// 0. Allow automatic downloading of nuget.exe
// 1. Configurable "got" directory
// 2. Configurable nuget path
// 3. Allow more packages than one (semi-colon separated string)
// 4. Allow configuration of what sources/feeds to get from
// 5. Be transparent regarding nuget console/error output
// 6. Allow prereleases to be omitted
// 7. Allow version ranges to be specified (as in AllowVersions 
// in packages.config when doing 'nuget update').

// Implementation
// 1. Just dont do freaking nuget install! (Use update in a shadow directory
// and keep track of what versions we've got).
// 3. Proper error handling / validation of arguments and context

namespace GoodGet {

    class Program {
        // Do nuget.exe install packageId, but with a tweak - the package is
        // always installed in a predictable path (i.e. not using version number)
        // and GoodGet makes sure it's always the last version that is installed.
        static void Main(string[] args) {
            Entrypoint(args);
            // Test
            // Entrypoint(new string[] { "NuGet.CommandLine" });
        }

        static void Entrypoint(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("Usage: goodget packageId [options]");
                return;
            }

            var watch = Stopwatch.StartNew();
            Console.WriteLine("Getting latest {0}...", args[0]);
            Execute(Path.Combine(Environment.CurrentDirectory, "nuget.exe"), args[0], Path.Combine(Environment.CurrentDirectory, "got"));
            watch.Stop();
            Console.WriteLine("Got in {0}s", watch.Elapsed.TotalSeconds);
        }

        static bool Execute(string nugetCommand, string package, string outputDir) {
            // 1) Under "got", keep a ".goodget" folder (hidden)
            // 2) For every package given, find the latest one in that directory.
            //    Just sort them based on semantic versioning algorithm.
            // 3) If a package is not there, install it using nuget install.
            //    Keep track of it as not being needed to update.
            // 4) For every package found, record it's current version in
            //    the 'update'-packages.config.
            // 5) If we have an 'update'-packages config, we should do an update.
            //    We save one copy of it, run update, and then compare the one
            //    that was issued by the update.
            // 6) Finally, synchronize the .goodget vault with the output folder,
            //    in where only the latest are to be installed (without version).
            //    Some strategy is needed to detect what is actually the version
            //    in the current out (compare time + size to latest in vault)?
            //
            // Suggested structure (with default "got" output directory)
            //
            // Out \got
            //       \.goodget  - root of shadow vault
            //         \got
            //         tmp.csproj
            //         packages.config
            //      Package1
            //      Package2

            var existing = Path.Combine(outputDir, package);
            if (Directory.Exists(existing)) {
                Directory.Delete(existing, true);
            }

            var output = new List<string>();
            var args = string.Format("install {0} -OutputDirectory {1} -NonInteractive -ExcludeVersion -Prerelease", package, outputDir);
            var start = new ProcessStartInfo(nugetCommand, args) {
                ErrorDialog = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

            var p = new Process() { StartInfo = start };
            p.OutputDataReceived += (sender, e) => { output.Add(e.Data); };

            // Verbose
            // Console.WriteLine(p.StartInfo.FileName + " " + p.StartInfo.Arguments);
            p.Start();
            p.BeginOutputReadLine();
            if (!p.HasExited) { 
                p.WaitForExit(); 
            }

            return p.ExitCode == 0;
        }
    }
}
