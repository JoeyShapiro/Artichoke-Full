using System;
using Artichoke.xamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Linq;

namespace Artichoke.xamarin.Services
{
	public static class API_Interface
	{
		private static string api_address = "10.0.2.2";
		private static string api_port = "6060";

		public static string ApiAddress { get => api_address; set => api_address = value; }
		public static string ApiPort { get => api_port; set => api_port = value; }

		public static async Task<(IEnumerable<Item>, Exception)> GetItemsLeft()
		{
			string url = "http://" + api_address + ":" + api_port + "/itemsleft";

			var values = new Dictionary<string, string>
			{
				{ "family_id", "1" },
				{ "passphrase_hash", "sha256" },
				{ "given_id", "1" }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var items = JsonConvert.DeserializeObject<IEnumerable<Item>>(result);
			items.ToList().ForEach(item => item.IsNotCollected = true);

			return (items, null);
		}

		public static async Task<(Family, Exception)> GetMyFamily()
		{
			string url = "http://" + api_address + ":" + api_port + "/myfamily";

			var values = new Dictionary<string, string>
			{
				{ "family_id", "1" },
				{ "passphrase_hash", "sha256" },
				{ "given_id", "1" }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var family = JsonConvert.DeserializeObject<Family>(result);

			return (family, null);
		}

		public static async Task<(IEnumerable<Category>, Exception)> GetCategories()
		{
			string url = "http://" + api_address + ":" + api_port + "/getcategories";

			var values = new Dictionary<string, string>
			{
				{ "family_id", "1" },
				{ "passphrase_hash", "sha256" },
				{ "given_id", "1" }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(result);

			return (categories, null);
		}

		public static async Task<(IEnumerable<Item>, Exception)> GetItemsCollected()
		{
			string url = "http://" + api_address + ":" + api_port + "/itemscollected";

			var values = new Dictionary<string, string>
			{
				{ "family_id", "1" },
				{ "passphrase_hash", "sha256" },
				{ "given_id", "1" }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var items = JsonConvert.DeserializeObject<IEnumerable<Item>>(result);

			return (items, null);
		}

		public static async Task<Exception> ItemCollect(Item item)
		{
			string url = "http://" + api_address + ":" + api_port + "/itemcollect";

			var values = new Dictionary<string, string>
			{
				{ "family_id", "1" },
				{ "passphrase_hash", "sha256" },
				{ "given_id", "1" },
				{ "item_id", item.Id }
			};

			(string result, Exception err) = await apiPostRequest(url, values);

			// maybe do something with result

			return err;
		}

		public static async Task<Exception> ItemAdd(Item item)
		{
			string url = "http://" + api_address + ":" + api_port + "/itemadd";

			var values = new Dictionary<string, string>
			{
				{ "family_id", "1" },
				{ "passphrase_hash", "sha256" },
				{ "given_id", "1" },
				{ "item_name", item.Name },
				{ "item_category_id", item.Category },
				{ "item_desc", item.Desc }
			};

			(string result, Exception err)= await apiPostRequest(url, values);
			if (err != null)
				return err;

			return null;
		}

		// generic api post request
		private static async Task<(string, Exception)> apiPostRequest(string url, Dictionary<string, string> keyValues)
		{
			string content;
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
					Exception err = new Exception(response.Content.ReadAsStringAsync().Result);
					return ("", err);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"\tERROR {0}", ex.Message);
				return ("", ex);
			}

			return (content, null);
		}
	}
}

