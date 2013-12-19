
namespace GoodGet {

    /// <summary>
    /// Expose the interface of a client that can get data
    /// and metadata from a given <see cref="Feed"/>.
    /// </summary>
    /// <remarks>
    /// This interface is designed to be kind of niched and
    /// only require methods for what GoodGet really need.
    /// After all, factoring out the REST client to use into
    /// an interface is all because how slow the .NET framework
    /// clients are (and we still want to support a standalone
    /// alternative, if performance can be sacrificed).
    /// </remarks>
    public interface IRestClient {
        /// <summary>
        /// Gets the <see cref="Feed"/> the current client
        /// should reach to when fetching package data and
        /// metadata.
        /// </summary>
        Feed Feed { get; }

        /// <summary>
        /// Gets a string in JSON format.
        /// </summary>
        /// <param name="uri">The uri to make the request too.
        /// </param>
        /// <returns>A string response in JSON format.</returns>
        string GetJSONString(string uri);
    }
}