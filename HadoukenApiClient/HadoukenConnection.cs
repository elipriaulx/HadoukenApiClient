using System;
using System.Net;

namespace HadoukenApiClient
{
    public class HadoukenConnection
    {
        #region  Properties

        public NetworkCredential Credentials { get; }
        public Uri Uri { get; }

        #endregion

        #region Constructors

        public HadoukenConnection(string path, string username, string password)
        {
            Credentials = new NetworkCredential(username, password);
            Uri = new Uri(path);
        }

        public HadoukenConnection(IPAddress address, uint port, string username, string password)
            : this($"http://{address}:{port}/api", username, password)
        {
        }

        #endregion
    }
}
