using System.IO;

namespace GoodGet {

    internal static class Utilities {

        public static void AssureDirectory(string path, FileAttributes attributes = FileAttributes.Normal) {
            if (!Directory.Exists(path)) {
                var d = Directory.CreateDirectory(path);
                if (attributes != FileAttributes.Normal) {
                    d.Attributes |= attributes;
                }
            }
        }
    }
}
