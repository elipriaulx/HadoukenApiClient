using System.Collections.Generic;
using System.Linq;
using HadoukenApiClient.Messages;
using Newtonsoft.Json.Linq;

namespace HadoukenApiClient
{
    public abstract class HadoukenClientBase
    {
        #region Constructors

        protected HadoukenClientBase()
        {
            
        }

        #endregion

        #region Protected Methods

        protected string ProduceSystemInfoQueryString()
        {
            return "{\"method\": \"core.getSystemInfo\", \"params\": [] }";
        }

        protected SystemInfo ParseGetSystemInfoResponse(JObject response)
        {
            var result = response["result"];

            var commitish = (string)result["commitish"];
            var branch = (string)result["branch"];
            var libTorrentVersion = (string)result["versions"]["libtorrent"];
            var hadoukenVersion = (string)result["versions"]["hadouken"];

            return new SystemInfo
            {
                Commitish = commitish,
                Branch = branch,
                LibTorrentVersion = libTorrentVersion,
                HadoukenVersion = hadoukenVersion
            };
        }

        protected string GetStateQueryString()
        {
            return "{\"jsonrpc\":\"2.0\",\"id\":1,\"method\":\"core.multiCall\",\"params\":{ \"webui.list\":[]}}";
        }

        protected IEnumerable<ItemSummary> ParseGetStateResponse(JObject response)
        {
            var result = response["result"]["webui.list"]["torrents"];

            var data = result.Select(c =>
            {
                var tItem = ((JArray)c).Select(i => i.ToString()).ToArray();

                return new ItemSummary
                {
                    InfoHash = tItem[0],
                    State = int.Parse(tItem[1]),
                    Name = tItem[2],
                    SizeBytes = long.Parse(tItem[3]),
                    Progress = double.Parse(tItem[4]) / 1000,
                    IsComplete = int.Parse(tItem[24]) - int.Parse(tItem[23]) > 0
                };
            }).ToList();

            return data;
        }

        protected string GetAddTorrentUrlQueryString(string url, bool paused)
        {
            return "{ \"method\": \"webui.addTorrent\", \"params\": [ \"url\", \"" + url + "\", { \"savePath\": 0, \"subPath\": \"\",\"paused\":" + paused.ToString().ToLower() + ",\"sequentialDownload\":false}]}";
        }

        protected bool ParseAddTorrentUrlResponse(JObject response)
        {
            // Todo: Confirm result integrity
            var result = response["result"];
            return true;
        }

        protected string GetRemoveQueryString(IEnumerable<string> infoHashs, bool removeData)
        {
            // Todo: Handle remove data
            var body = string.Join("\",\"", infoHashs);
            return  "{ \"jsonrpc\":\"2.0\",\"id\":1,\"method\":\"core.multiCall\",\"params\":{ \"webui.list\":[],\"webui.perform\":[\"remove\",[\"" + body + "\"]]}}";
        }

        protected bool ParseRemoveResponse(JObject response)
        {
            // Todo: Confirm result integrity
            var result = response["result"];
            return true;
        }


        protected string GetStartQueryString(IEnumerable<string> infoHashs, bool forceStart)
        {
            var startString = forceStart ? "forcestart" : "start";
            var body = string.Join("\",\"", infoHashs);
            return "{ \"jsonrpc\":\"2.0\",\"id\":1,\"method\":\"core.multiCall\",\"params\":{ \"webui.list\":[],\"webui.perform\":[\""+  forceStart +"\",[\"" + body + "\"]]}}";
        }

        protected bool ParseStartResponse(JObject response)
        {
            // Todo: Confirm result integrity
            var result = response["result"];
            return true;
        }

        protected string GetPauseQueryString(IEnumerable<string> infoHashs)
        {
            var body = string.Join("\",\"", infoHashs);
            return "{ \"jsonrpc\":\"2.0\",\"id\":1,\"method\":\"core.multiCall\",\"params\":{ \"webui.list\":[],\"webui.perform\":[\"forcestart\",[\"" + body + "\"]]}}";
        }

        protected bool ParsePauseResponse(JObject response)
        {
            // Todo: Confirm result integrity
            var result = response["result"];
            return true;
        }

        #endregion
    }
}
