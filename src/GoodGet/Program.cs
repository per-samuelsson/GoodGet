
using Modules;
using System;
using System.Collections.Generic;

namespace GoodGet {

    class Program {

        static void Main(string[] args) {
            if (args.Length < 2) {
                Usage();
                return;
            }

            var packagesFolder = string.Empty;
            var packages = new List<string>();
            var verbose = false;
            var quiet = false;
            var time = false;

            foreach (var arg in args) {
                if (arg.StartsWith("-")) {
                    var option = arg.Substring(1).ToLower();
                    switch (option) {
                        case "verbose":
                            verbose = true;
                            break;
                        case "quiet":
                            quiet = true;
                            break;
                        case "time":
                            time = true;
                            break;
                        case "got":
                            Console.WriteLine("Option 'got' is not yet supported");
                            return;
                        default:
                            Console.WriteLine("Option {0} not recognized", option);
                            Usage();
                            return;
                    }
                } else if (packagesFolder == string.Empty) {
                    packagesFolder = arg;
                } else {
                    packages.Add(arg);
                }
            }

            if (packagesFolder == string.Empty) {
                Console.WriteLine("No packages folder given");
                Usage();
                return;
            } else if (packages.Count == 0) {
                Console.WriteLine("Specify at least one package");
                Usage();
                return;
            }

            var console = GoodGetModule.Injections.Console;
            if (quiet || verbose || time) {
                var consoleImplementation = console as StandardConsole;
                if (consoleImplementation != null) {
                    if (quiet) {
                        consoleImplementation.Quiet = true;
                    } else if (verbose) {
                        consoleImplementation.CurrentSeverityLevel = Rank.Debug;
                    }

                    if (time) {
                        consoleImplementation.ShowTime = true;
                    }
                }
            }

            var result = PackagesFolder.InstallPackages(packagesFolder, packages.ToArray());
            if (!result) {
                console.WriteLine("All packages up-to-date.");
            }
        }

        static void Usage() {
            Console.WriteLine("Usage:");
            Console.WriteLine("  goodget [options] folder <<package1>, [<package2>, <...>]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -verbose  Write verbose output");
            Console.WriteLine("  -quiet    Write no output");
            Console.WriteLine("  -time     Prefix all output with time elapsed since starting");
            Console.WriteLine("  -got      Just show what packages we got");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  goodget . NUnit");
            Console.WriteLine("  goodget C:\\packages NUnit xUnit jQuery");
        }
    }
}