namespace HadoukenApiClient.Messages
{
    public class ItemSummary
    {
        #region  Properties

        public string InfoHash { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public long SizeBytes { get; set; }
        public double Progress { get; set; }
        public bool IsComplete { get; set; }

        #endregion
    }
}
