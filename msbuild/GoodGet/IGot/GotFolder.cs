using System;
using System.Globalization;
using System.IO;

namespace GoodGet {

    /// <summary>
    /// Implements <see cref="IGot"/> using a hidden 
    /// subdirectory to the packages directory in where
    /// all package metadata is stored.
    /// </summary>
    internal sealed class GotFolder : IGot {
        const string name = ".goodgot";
        string path;
        string packagesPath;

        /// <summary>
        /// Serialize package version information to and from
        /// <see cref="Package"/> instances.
        /// </summary>
        /// <remarks>
        /// Current format: Version Time, e.g "1.2.3-alpha 2013121710:08:03"
        /// </remarks>
        static class PackageSerializer {
            const string timeFormat = "yyyyMMddHH:mm:ss";

            public static void DeserializePackage(string package, string data, out Package got) {
                got = null;
                var tokens = data.Split(' ');
                if (tokens.Length >= 2) {
                    DateTime time;
                    if (DateTime.TryParseExact(tokens[1], timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out time)) {
                        got = new Package() {
                            Id = package,
                            Version = tokens[0],
                            Retreived = time,
                        };
                    }
                }
            }

            public static void SerializePackage(Package package, out string data) {
                data = string.Format("{0} {1}", package.Version, package.Retreived.Value.ToString(timeFormat));
            }
        }

        public GotFolder(PackagesFolder packagesFolder) {
            packagesPath = packagesFolder.Path;
            path = Path.Combine(packagesPath, name);
        }

        Package[] IGot.Get(string[] packages) {
            var got = new Package[packages.Length];
            if (Directory.Exists(packagesPath) && Directory.Exists(path)) {
                for (int i = 0; i < packages.Length; i++) {
                    GetPackage(packages[i], out got[i]);
                }
            }
            return got;
        }

        void IGot.Save(Package package) {
            AssureDirectory(packagesPath);
            AssureDirectory(path, FileAttributes.Hidden);

            if (package.Retreived == null) {
                package.Retreived = DateTime.Now;
            }

            var pkgFile = GetGotFilePath(package.Id);
            File.Delete(pkgFile);

            string packageInfo;
            PackageSerializer.SerializePackage(package, out packageInfo);
            File.WriteAllText(pkgFile, packageInfo);
        }

        void GetPackage(string package, out Package got) {
            got = null;
            var pkgFile = GetGotFilePath(package);
            if (File.Exists(pkgFile)) {
                var content = File.ReadAllLines(pkgFile);
                if (content.Length >= 1) {
                    PackageSerializer.DeserializePackage(package, content[0], out got);
                }
            }
        }

        string GetGotFilePath(string package) {
            return Path.Combine(path, package);
        }

        void AssureDirectory(string path, FileAttributes attributes = FileAttributes.Normal) {
            if (!Directory.Exists(path)) {
                var d = Directory.CreateDirectory(path);
                if (attributes != FileAttributes.Normal) {
                    d.Attributes |= attributes;
                }
            }
        }
    }
}