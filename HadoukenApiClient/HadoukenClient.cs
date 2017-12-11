using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HadoukenApiClient.Exceptions;
using HadoukenApiClient.Messages;
using Newtonsoft.Json.Linq;

namespace HadoukenApiClient
{
    public class HadoukenClient : HadoukenClientBase
    {
        #region  Fields

        private readonly HadoukenConnection _connection;

        #endregion

        #region Constructors

        public HadoukenClient(HadoukenConnection connection)
        {
            _connection = connection ?? throw new ClientException("A connection instance must be provided when creating a client.");
        }

        #endregion

        #region Public Methods

        public SystemInfo GetSystemInfo()
        {
            try
            {
                var json = ProduceSystemInfoQueryString();
                var jObject = Request(json);
                return ParseGetSystemInfoResponse(jObject);
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ClientException("Unable to complete request.", ex);
            }
        }

        public IEnumerable<ItemSummary> GetState()
        {
            try
            {
                var json = GetStateQueryString();
                var jObject = Request(json);
                return ParseGetStateResponse(jObject);
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ClientException("Unable to complete request.", ex);
            }
        }

        public bool AddTorrentUrl(string url, bool paused = false)
        {
            try
            {
                var json = GetAddTorrentUrlQueryString(url, paused);
                var jObject = Request(json);
                return ParseAddTorrentUrlResponse(jObject);
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ClientException("Unable to complete request.", ex);
            }
        }

        public bool Remove(ItemSummary summary, bool removeData = false)
        {
            return Remove(summary.InfoHash, removeData);
        }

        public bool Remove(string infoHash, bool removeData = false)
        {
            return Remove(new[] { infoHash }, removeData);
        }

        public bool Remove(IEnumerable<ItemSummary> infoHashs, bool removeData = false)
        {
            var h = infoHashs.Select(x => x.InfoHash).ToArray();
            return Remove(h, removeData);
        }

        public bool Remove(IEnumerable<string> infoHashs, bool removeData = false)
        {
            try
            {
                var json = GetRemoveQueryString(infoHashs, removeData);
                var jObject = Request(json);
                return ParseRemoveResponse(jObject);
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ClientException("Unable to complete request.", ex);
            }
        }

        public bool Start(ItemSummary summary, bool forceStart = false)
        {
            return Start(summary.InfoHash, forceStart);
        }

        public bool Start(string infoHash, bool forceStart = false)
        {
            return Start(new[] { infoHash }, forceStart);
        }

        public bool Start(IEnumerable<ItemSummary> infoHashs, bool forceStart = false)
        {
            var h = infoHashs.Select(x => x.InfoHash).ToArray();
            return Start(h, forceStart);
        }

        public bool Start(IEnumerable<string> infoHashs, bool forceStart = false)
        {
            try
            {
                var json = GetStartQueryString(infoHashs, forceStart);
                var jObject = Request(json);
                return ParseStartResponse(jObject);
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ClientException("Unable to complete request.", ex);
            }
        }

        public bool Pause(ItemSummary summary)
        {
            return Pause(summary.InfoHash);
        }

        public bool Pause(string infoHash)
        {
            return Pause(new[] { infoHash });
        }

        public bool Pause(IEnumerable<ItemSummary> infoHashs)
        {
            var h = infoHashs.Select(x => x.InfoHash).ToArray();
            return Pause(h);
        }

        public bool Pause(IEnumerable<string> infoHashs)
        {
            try
            {
                var json = GetPauseQueryString(infoHashs);
                var jObject = Request(json);
                return ParsePauseResponse(jObject);
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ClientException("Unable to complete request.", ex);
            }
        }

        #endregion

        #region Private Methods

        private JObject Request(string requestData)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Credentials = _connection.Credentials;

                    var response = webClient.UploadString(_connection.Uri, "POST", requestData);

                    return JObject.Parse(response);
                }
            }
            catch (WebException ex)
            {
                throw new ClientException("Unable to complete request.", ex);
            }
        }

        #endregion
    }
}
