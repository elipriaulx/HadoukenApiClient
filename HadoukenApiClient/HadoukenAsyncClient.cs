using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HadoukenApiClient.Exceptions;
using HadoukenApiClient.Messages;
using Newtonsoft.Json.Linq;

namespace HadoukenApiClient
{
    public class HadoukenAsyncClient : HadoukenClientBase
    {
        #region  Fields

        private readonly HadoukenConnection _connection;

        #endregion

        #region Constructors

        public HadoukenAsyncClient(HadoukenConnection connection)
        {
            _connection = connection ?? throw new ClientException("A connection instance must be provided when creating a client.");
        }

        #endregion

        #region Public Methods

        public async Task<SystemInfo> GetSystemInfo()
        {
            try
            {
                var json = ProduceSystemInfoQueryString();
                var jObject = await RequestAsync(json);
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

        public async Task<IEnumerable<ItemSummary>> GetState()
        {
            try
            {
                var json = GetStateQueryString();
                var jObject = await RequestAsync(json);
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

        public async Task<bool> AddTorrentUrl(string url, bool paused = false)
        {
            try
            {
                var json = GetAddTorrentUrlQueryString(url, paused);
                var jObject = await RequestAsync(json);
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

        public async Task<bool> Remove(ItemSummary summary, bool removeData = false)
        {
            return await Remove(summary.InfoHash, removeData);
        }

        public async Task<bool> Remove(string infoHash, bool removeData = false)
        {
            return await Remove(new [] { infoHash }, removeData);
        }

        public async Task<bool> Remove(IEnumerable<ItemSummary> infoHashs, bool removeData = false)
        {
            var h = infoHashs.Select(x => x.InfoHash).ToArray();
            return await Remove(h, removeData);
        }

        public async Task<bool> Remove(IEnumerable<string> infoHashs, bool removeData = false)
        {
            try
            {
                var json = GetRemoveQueryString(infoHashs, removeData);
                var jObject = await RequestAsync(json);
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

        public async Task<bool> Start(ItemSummary summary, bool forceStart = false)
        {
            return await Start(summary.InfoHash, forceStart);
        }

        public async Task<bool> Start(string infoHash, bool forceStart = false)
        {
            return await Start(new[] { infoHash }, forceStart);
        }

        public async Task<bool> Start(IEnumerable<ItemSummary> infoHashs, bool forceStart = false)
        {
            var h = infoHashs.Select(x => x.InfoHash).ToArray();
            return await Start(h, forceStart);
        }

        public async Task<bool> Start(IEnumerable<string> infoHashs, bool forceStart = false)
        {
            try
            {
                var json = GetStartQueryString(infoHashs, forceStart);
                var jObject = await RequestAsync(json);
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

        public async Task<bool> Pause(ItemSummary summary)
        {
            return await Pause(summary.InfoHash);
        }

        public async Task<bool> Pause(string infoHash)
        {
            return await Pause(new[] { infoHash });
        }

        public async Task<bool> Pause(IEnumerable<ItemSummary> infoHashs)
        {
            var h = infoHashs.Select(x => x.InfoHash).ToArray();
            return await Pause(h);
        }

        public async Task<bool> Pause(IEnumerable<string> infoHashs)
        {
            try
            {
                var json = GetPauseQueryString(infoHashs);
                var jObject = await RequestAsync(json);
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

        private async Task<JObject> RequestAsync(string requestData)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Credentials = _connection.Credentials;

                    var response = await webClient.UploadStringTaskAsync(_connection.Uri, "POST", requestData);

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
