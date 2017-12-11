namespace HadoukenApiClient.Messages
{
    public class SystemInfo
    {
        #region  Properties

        public string Commitish { get; set; }
        public string Branch { get; set; }
        public string LibTorrentVersion { get; set; }
        public string HadoukenVersion { get; set; }

        #endregion
    }
}
