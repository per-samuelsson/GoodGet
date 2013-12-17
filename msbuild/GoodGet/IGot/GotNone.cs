
namespace GoodGet {

    /// <summary>
    /// Implements <see cref="IGet"/> always returning no 
    /// packages are installed whatsoever.
    /// </summary>
    internal class GotNone : IGot {

        Package[] IGot.Get(string[] packages) {
            return new Package[packages.Length];
        }

        void IGot.Save(Package package) {
            // Nothing
        }
    }
}
