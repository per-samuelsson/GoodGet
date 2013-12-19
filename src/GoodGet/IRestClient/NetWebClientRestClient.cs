
using System;
using System.Net;

namespace GoodGet {
    /// <summary>
    /// Implements the <see cref="IRestClient"/> interface by using the
    /// BCL <see cref="WebClient"/> API.
    /// </summary>
    /// <remarks>
    /// Performance of this is just... gzzz.
    /// </remarks>
    internal sealed class NetWebClientRestClient : IRestClient {
        readonly Feed feed;
        readonly WebClient client;

        /// <summary>
        /// Implements the <see cref="IRestClientFactory"/> producing
        /// <see cref="IRestClient"/> instances using the
        /// <see cref="NetWebClientRestClient"/> implementation.
        /// </summary>
        public sealed class Factory : IRestClientFactory {

            /// <inheritdoc/>
            IRestClient IRestClientFactory.CreateClient(Feed feed) {
                return new NetWebClientRestClient(feed);
            }
        }

        /// <summary>
        /// Initialize an instance of <see cref="NetWebClientRestClient"/>.
        /// </summary>
        /// <param name="feed">The feed the client will request data
        /// from.</param>
        private NetWebClientRestClient(Feed feed) {
            this.feed = feed;
            
            client = new WebClient();
            client.Headers.Add("accept:application/json");
        }

        /// <inheritdoc/>
        Feed IRestClient.Feed {
            get { return feed; }
        }

        /// <inheritdoc/>
        string IRestClient.GetJSONString(string uri) {
            return client.DownloadString(uri);
        }
    }
}
