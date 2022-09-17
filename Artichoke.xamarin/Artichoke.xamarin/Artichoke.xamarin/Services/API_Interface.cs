using System;
using Artichoke.xamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace Artichoke.xamarin.Services
{
	public static class API_Interface
	{
		private static string api_address = "10.0.2.2";
		private static string api_port = "6060";

		public static string ApiAddress { get => api_address; set => api_address = value; }
		public static string ApiPort { get => api_port; set => api_port = value; }

        public static async Task<IEnumerable<Item>> GetItemsLeft()
		{
            string url = "http://" + api_address + ":" + api_port + "/itemsleft";

            var values = new Dictionary<string, string>
            {
                { "family_id", "1" },
                { "passphrase_hash", "sha256" },
                { "given_id", "1" }
            };

            string result = await apiPostRequest(url, values);

            var items = JsonConvert.DeserializeObject<IEnumerable<Item>>(result);

            return items;
        }

		public static async Task<Family> GetMyFamily()
		{
			string url = "http://" + api_address + ":" + api_port + "/myfamily";

			var values = new Dictionary<string, string>
			{
				{ "family_id", "1" },
				{ "passphrase_hash", "sha256" },
				{ "given_id", "1" }
			};

			string result = await apiPostRequest(url, values);

			var family = JsonConvert.DeserializeObject<Family>(result);

			return family;
		}

		private static async Task<string> apiPostRequest(string url, Dictionary<string, string> keyValues)
		{
			string content = string.Empty;
			HttpClient client = new HttpClient();
			Uri uri = new Uri(url);

			try
			{
				HttpResponseMessage response = await client.PostAsJsonAsync(uri, keyValues);
				if (response.IsSuccessStatusCode)
				{
					content = await response.Content.ReadAsStringAsync();
				}
				else
				{
					content = "no";
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"\tERROR {0}", ex.Message);
			}

			return content;
		}
	}
}

