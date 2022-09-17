using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Artichoke.xamarin.Models
{
	public class Family
	{
		[JsonProperty("family")]
		public string Name { get; set; }
		[JsonProperty("expires_on")]
		public string SubExpirationUnix { get; set; }
		public string SubExpiration { get; set; }
		[JsonProperty("members")]
		public IEnumerable<Member> Members { get; set; }

		public Family()
		{
			Name = "";
			SubExpiration = "0";
			Members = new List<Member>();
		}
    }
}

