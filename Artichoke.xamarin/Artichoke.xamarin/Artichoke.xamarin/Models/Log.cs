using System;
using Newtonsoft.Json;

namespace Artichoke.xamarin.Models
{
    public class Log
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("item")]
        public string Item { get; set; }
        [JsonProperty("modified_on")]
        public string Modified_On { get; set; }

        public string Formatted_Log { get { return String.Format("{0} has {1} {2} at {3}", Username, Action, Item, Modified_On); } } // good enough, use MultiBindings later
    }
}
