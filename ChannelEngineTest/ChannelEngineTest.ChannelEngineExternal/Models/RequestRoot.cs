namespace ChannelEngineTest.ChannelEngineExternal.Models
{
    public class RequestRoot<T>
    {
        public T Content { get; set; }
        
        public int Count { get; set; }
        
        public int TotalCount { get; set; }
        
        public int ItemsPerPage { get; set; }
        
        public int StatusCode { get; set; }
        
        public bool Success { get; set; }
        
        public string Message { get; set; }
    }
}