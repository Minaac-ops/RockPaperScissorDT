using System.Collections.Generic;

namespace Events
{
    public class TracingEventBase
    {
        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();
        

    }
}