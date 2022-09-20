using System;
using Newtonsoft.Json;

namespace Artichoke.xamarin.Models
{
    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("item")]
        public string Name { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("description")]
        public string Desc { get; set; }
        public bool IsNotCollected { get; set; } //? this is for the swipeview
    }
}
