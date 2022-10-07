using System;
using Newtonsoft.Json;

namespace Artichoke.xamarin.Models
{
	public class Account
	{
        [JsonProperty("given_id")]
        public string GivenId { get; set; }
		[JsonProperty("given_passphrase_hash")]
		public string GivenPassphraseHash { get; set; }
		[JsonProperty("family_id")]
		public string FamilyId { get; set; }
        [JsonProperty("family_passphrase_hash")]
        public string FamilyPassphraseHash { get; set; }
	}
}