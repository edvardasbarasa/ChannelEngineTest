namespace ChannelEngineTest.ChannelEngineExternal.Models
{
    public class ChannelEngineSettings
    {
        public string Key { get; set; }

        public string BaseAddress { get; set; }
        
        public int RetriesCount { get; set; }

        public int ProgressiveRetryDelayMs { get; set; }
    }
}