using System.Collections.Generic;

namespace DynamicQL.Core
{
    public class DynamicQLRequest
    {
        public string Table { get; set; }

        public string Query { get; set; }

        public Dictionary<string, object> Variables { get; set; }
    }
}
