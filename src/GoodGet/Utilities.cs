
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GoodGet {

    internal static class Utilities {

        public static class ToolProcess {

            public static int Invoke(ProcessStartInfo processStartInfo, List<string> output = null, List<string> errorOutput = null) {
                processStartInfo.ErrorDialog = false;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardOutput = true;
                var process = new Process {
                    StartInfo = processStartInfo
                };
                if (output != null) {
                    process.OutputDataReceived += (sender, e) => { output.Add(e.Data); };
                }
                if (errorOutput != null) {
                    process.ErrorDataReceived += (sender, e) => { errorOutput.Add(e.Data); };
                }

                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();

                return process.ExitCode;
            }
        }

        public static void AssureDirectory(string path, FileAttributes attributes = FileAttributes.Normal) {
            if (!Directory.Exists(path)) {
                var d = Directory.CreateDirectory(path);
                if (attributes != FileAttributes.Normal) {
                    d.Attributes |= attributes;
                }
            }
        }

        public static T[] Clone<T>(this ICloneable[] array) {
            var result = new T[array.Length];
            for (int i = 0; i < array.Length; i++) {
                result[i] = (T) array[i].Clone();
            }
            return result;
        }
    }
}
