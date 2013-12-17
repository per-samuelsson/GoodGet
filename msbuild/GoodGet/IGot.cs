
namespace GoodGet {
    /// <summary>
    /// Expose the packages GoodGet got, including their version.
    /// </summary>
    public interface IGot {
        /// <summary>
        /// Gets the set of packages specified in <paramref name="packages"/>.
        /// </summary>
        /// <param name="packages">The packages to retreive.</param>
        /// <returns>An array of <see cref="Package"/>s.</returns>
        public Package[] Get(string[] packages);
        
        /// <summary>
        /// Saves the current package information for later retreival.
        /// </summary>
        /// <param name="package">The <see cref="Package"/> to save.</param>
        public void Save(Package package);
    }
}