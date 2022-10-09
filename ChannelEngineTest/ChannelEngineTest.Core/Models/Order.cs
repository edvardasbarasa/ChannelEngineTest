using System.Collections.Generic;

namespace ChannelEngineTest.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public List<Line> Lines { get; set; }
    }
}