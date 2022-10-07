using System;
using Artichoke.xamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Essentials;

namespace Artichoke.xamarin.Services
{
	public static class API_Interface
	{
		public static string api_address =
			DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
		//private static string api_address = "10.0.2.2"; // maybe move these to account
		private static string api_port = "6060";

		public static string ApiAddress { get => api_address; set => api_address = value; }
		public static string ApiPort { get => api_port; set => api_port = value; }

		public static async Task<(IEnumerable<Item>, Exception)> GetItemsLeft()
		{
			string url = "https://" + api_address + ":" + api_port + "/itemsleft";

			var values = new Dictionary<string, string>
			{
				{ "family_id", Services.Settings.FamilyID },
				{ "passphrase_hash", Services.Settings.FamilyPassphraseHash },
				{ "given_id", Services.Settings.GivenID }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			if (result.Length < 5)
				return (new List<Item>(), null);

			var items = JsonConvert.DeserializeObject<IEnumerable<Item>>(result);
			items.ToList().ForEach(item => item.IsNotCollected = true);

			return (items, null);
		}

		public static async Task<(Account, Exception)> GetAccount(string given_id, string given_passphrase_hash)
		{
			string url = "https://" + api_address + ":" + api_port + "/getaccount";

			var values = new Dictionary<string, string>
			{
				{ "given_id", given_id },
				{ "given_passphrase_hash", given_passphrase_hash }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var account = JsonConvert.DeserializeObject<Account>(result);

			return (account, null);
		}

		public static async Task<(Family, Exception)> GetMyFamily()
		{
			string url = "https://" + api_address + ":" + api_port + "/myfamily";

			var values = new Dictionary<string, string>
			{
				{ "family_id", Services.Settings.FamilyID },
				{ "passphrase_hash", Services.Settings.FamilyPassphraseHash },
				{ "given_id", Services.Settings.GivenID }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var family = JsonConvert.DeserializeObject<Family>(result);

			return (family, null);
		}

		public static async Task<(IEnumerable<Log>, Exception)> GetFamilyLogs()
		{
			string url = "https://" + api_address + ":" + api_port + "/getfamilylogs";

			var values = new Dictionary<string, string>
			{
				{ "family_id", Services.Settings.FamilyID },
				{ "passphrase_hash", Services.Settings.FamilyPassphraseHash },
				{ "given_id", Services.Settings.GivenID }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var logs = JsonConvert.DeserializeObject<IEnumerable<Log>>(result);

			return (logs, null);
		}

		public static async Task<(IEnumerable<Category>, Exception)> GetCategories()
		{
			string url = "https://" + api_address + ":" + api_port + "/getcategories";

			var values = new Dictionary<string, string>
			{
				{ "family_id", Services.Settings.FamilyID },
				{ "passphrase_hash", Services.Settings.FamilyPassphraseHash },
				{ "given_id", Services.Settings.GivenID }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(result);

			return (categories, null);
		}

		public static async Task<(IEnumerable<Item>, Exception)> GetItemsCollected()
		{
			string url = "https://" + api_address + ":" + api_port + "/itemscollected";

			var values = new Dictionary<string, string>
			{
				{ "family_id", Services.Settings.FamilyID },
				{ "passphrase_hash", Services.Settings.FamilyPassphraseHash },
				{ "given_id", Services.Settings.GivenID }
			};

			(string result, Exception err) = await apiPostRequest(url, values);
			if (err != null)
				return (null, err);

			var items = JsonConvert.DeserializeObject<IEnumerable<Item>>(result);

			return (items, null);
		}

		public static async Task<Exception> ItemCollect(Item item)
		{
			string url = "https://" + api_address + ":" + api_port + "/itemcollect";

			var values = new Dictionary<string, string>
			{
				{ "family_id", Services.Settings.FamilyID },
				{ "passphrase_hash", Services.Settings.FamilyPassphraseHash },
				{ "given_id", Services.Settings.GivenID },
				{ "item_id", item.Id }
			};

			(string result, Exception err) = await apiPostRequest(url, values);

			// maybe do something with result

			return err;
		}

		public static async Task<Exception> ItemAdd(Item item, int category_id)
		{
			string url = "https://" + api_address + ":" + api_port + "/itemadd";

			var values = new Dictionary<string, string>
			{
				{ "family_id", Services.Settings.FamilyID },
				{ "passphrase_hash", Services.Settings.FamilyPassphraseHash },
				{ "given_id", Services.Settings.GivenID },
				{ "item_name", item.Name },
				{ "item_category_id", category_id.ToString() },
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
			HttpClient client = new HttpClient(GetInsecureHandler());
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

		// This method must be in a class in a platform project, even if
		// the HttpClient object is constructed in a shared project.
		public static HttpClientHandler GetInsecureHandler()
		{
			HttpClientHandler handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
			{
				if (cert.Issuer.Equals("O=Internet Widgits Pty Ltd, S=Some-State, C=AU"))
					return true;
				return errors == System.Net.Security.SslPolicyErrors.None;
			};
			return handler;
		}
	}
}

