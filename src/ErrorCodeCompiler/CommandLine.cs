using System;
using System.IO;

namespace ErrorCodeCompiler {

    static class CommandLine {

        public static void ParseArgs(string[] args, ref Stream instream, ref TextWriter csfile, ref TextWriter orangestdcsfile, ref TextWriter orangeintcsfile, ref TextWriter mcfile, ref TextWriter exceptionAssistantContentFile) {
            if (args.Length == 0) {
                PrintUsageAndExit();
            }
            DateTime inputLastModified;
            string fn = args[0];
            OpenInStream(fn, ref instream, out inputLastModified);
            for (int i = 1; i < args.Length; i++) {
                switch (args[i]) {
                    case "-v": break; // handled elsewhere
                    case "-cs":
                        GetCSharpStream(args, inputLastModified, ref csfile, ref i);
                        break;
                    case "-orangestdcs":
                        GetCSharpStream2(args, inputLastModified, ref orangestdcsfile, ref i);
                        break;
                    case "-orangeintcs":
                        GetCSharpStream2(args, inputLastModified, ref orangeintcsfile, ref i);
                        break;
                    case "-mc":
                        GetMcStream(args, inputLastModified, ref mcfile, ref i);
                        break;
                    case "-ea":
                        GetEAContentStream(args, ref exceptionAssistantContentFile, ref i);
                        break;
                    default:
                        Program.Exit("Unknown switch: " + args[i]);
                        break;
                }
            }
        }


        private static void PrintUsageAndExit() {
            Console.Error.WriteLine("Usage:");
            Console.Error.WriteLine("    {0} infile.xml [options]", System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            Console.Error.WriteLine("Where [options] are:");
            Console.Error.WriteLine("-v             Verbose mode");
            Console.Error.WriteLine("-mc [mcfile]   Write native resource message file to \"mcfile\"");
            Console.Error.WriteLine("-cs [csfile]   Write constants to C# file \"csfile\"");
            Console.Error.WriteLine("-ea [contentfile]   Write VS exception assistant content file to \"contentfile\"");
            Program.Exit("Filenames can be - which means stdin/stdout");
        }

        private static void OpenInStream(string fn, ref Stream instream, out DateTime inputLastModified) {
            if (fn == "-") {
                Program.Verbose("Reading input from stdin");
                instream = Console.OpenStandardInput();
                inputLastModified = DateTime.MaxValue;
            } else {
                Program.Verbose("Reading input from {0}", fn);
                FileInfo fi = new FileInfo(fn);
                inputLastModified = fi.LastWriteTime;
                instream = fi.OpenRead();
            }
        }

        private static void GetCSharpStream(string[] args, DateTime inputLastModified, ref TextWriter csfile, ref int i) {
            if (++i == args.Length) {
                Program.Exit("-cs switch requires a filename argument");
            } else if (csfile != null) {
                Program.Exit("C# file already opened");
            } else if (args[i] == "-") {
                Program.Verbose("Using stdout for C# output");
                csfile = Console.Out;
            } else {
                Program.Verbose("Using file {0} for C# output", args[i]);
                csfile = OpenWritableIfYounger(args[i], inputLastModified);
            }
        }

        private static void GetCSharpStream2(string[] args, DateTime inputLastModified, ref TextWriter csfile, ref int i) {
            if (++i == args.Length) {
                Program.Exit("-cs2 switch requires a filename argument");
            } else if (csfile != null) {
                Program.Exit("C# file already opened");
            } else if (args[i] == "-") {
                Program.Verbose("Using stdout for C# output");
                csfile = Console.Out;
            } else {
                Program.Verbose("Using file {0} for C# output", args[i]);
                csfile = OpenWritableIfYounger(args[i], inputLastModified);
            }
        }

        private static void GetMcStream(string[] args, DateTime inputLastModified, ref TextWriter mcfile, ref int i) {
            if (++i == args.Length) {
                Program.Exit("-mc switch requires a filename argument");
            } else if (mcfile != null) {
                Program.Exit("MC file already opened");
            } else if (args[i] == "-") {
                Program.Verbose("Using stdout for MC output");
                mcfile = Console.Out;
            } else {
                Program.Verbose("Using file {0} for MC output", args[i]);
                mcfile = OpenWritableIfYounger(args[i], inputLastModified);
            }
        }

        private static void GetEAContentStream(string[] args, ref TextWriter contentFile, ref int i) {
            if (++i == args.Length) {
                Program.Exit("-ea switch requires a filename argument");
            } else if (contentFile != null) {
                Program.Exit("Excepction assistant content file already opened");
            } else if (args[i] == "-") {
                Program.Verbose("Using stdout for exception assistant content output");
                contentFile = Console.Out;
            } else {
                Program.Verbose("Using file {0} for exception assistant content output", args[i]);
                contentFile = OpenWritableIfYounger(args[i], DateTime.MaxValue);
            }
        }

        private static StreamWriter OpenWritableIfYounger(string fn, DateTime inputLastModified) {
            FileInfo fi = new FileInfo(fn);
            if (!fi.Exists) {
                Program.Verbose("Output file {0} does not exist, creating", fi.FullName);
                return new StreamWriter(fi.Create());
            }
#if false
        else if (fi.LastWriteTime > inputLastModified)
        {
            Program.Verbose("Output file {0} is more recent than input, skipping", fi.FullName);
            return null;
        }
#endif
 else if (!fi.IsReadOnly) {
                Program.Verbose("Output file {0} is less recent than input, opening for write", fi.FullName);
                return new StreamWriter(fi.Open(FileMode.Truncate, FileAccess.Write));
            } else {
                Program.Exit("File " + fn + " is read-only. Did you remember to check it out?");
            }
            throw new Exception("Die exits the program so this line should never be reached");
        }
    }
}
