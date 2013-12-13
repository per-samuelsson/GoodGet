
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
