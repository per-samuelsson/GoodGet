
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

            if (quiet || verbose) {
                var consoleImplementation = GoodGetModule.Injections.Console as StandardConsole;
                if (consoleImplementation != null) {
                    if (quiet) {
                        consoleImplementation.Quiet = true;
                    } else {
                        consoleImplementation.CurrentSeverityLevel = StandardConsole.Severity.Debug;
                    }
                }
            }

            PackagesFolder.InstallPackages(packagesFolder, packages.ToArray());
        }

        static void Usage() {
            Console.WriteLine("Usage:");
            Console.WriteLine("  goodget [options] folder <<package1>, [<package2>, <...>]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -verbose  Write verbose output");
            Console.WriteLine("  -quiet    Write no output");
            Console.WriteLine("  -got      Just show what packages we got");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  goodget . NUnit");
            Console.WriteLine("  goodget C:\\packages NUnit xUnit jQuery");
        }
    }
}