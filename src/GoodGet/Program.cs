
using System;

namespace GoodGet {

    class Program {

        static void Main(string[] args) {
            if (args.Length < 2) {
                Usage();
                return;
            }

            throw new NotImplementedException();
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