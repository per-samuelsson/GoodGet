
namespace GoodGet {
    /// <summary>
    /// Represent a factory that knows how to construct
    /// <see cref="IRestClient"/> instances.
    /// </summary>
    public interface IRestClientFactory {
        /// <summary>
        /// Creates an instance of a class implementing the
        /// <see cref="IRestClient"/> interface.
        /// </summary>
        /// <param name="feed">The <see cref="Feed"/> the client
        /// should request it's data from.</param>
        /// <returns>Instance of a <see cref="IRestClient"/>.</returns>
        IRestClient CreateClient(Feed feed);
    }
}