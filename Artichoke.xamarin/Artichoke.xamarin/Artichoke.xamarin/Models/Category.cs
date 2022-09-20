using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Artichoke.xamarin.Models
{
	public class Category
	{
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        public Category() { }
	}
}

