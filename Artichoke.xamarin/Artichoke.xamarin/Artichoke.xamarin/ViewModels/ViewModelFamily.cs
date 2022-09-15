using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artichoke.xamarin.ViewModels
{
    public class ViewModelFamily : BaseViewModel
	{
		public string result = "hello";
		public string Result { get { return result; } set { SetProperty(ref result, value); OnPropertyChanged("Result"); } }

		public ViewModelFamily()
		{
			
		}

		public async Task<string> RefreshDataAsync()
		{
			string content = string.Empty;
			HttpClient client = new HttpClient();
			Uri uri = new Uri("http://10.0.2.2:6060/myfamily");
			var values = new Dictionary<string, string>
			{
				{ "family_id", "1" },
				{ "passphrase_hash", "sha256" },
				{ "given_id", "1" }
			};

			try
			{
				HttpResponseMessage response = await client.PostAsJsonAsync(uri, values);
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

			result = content;

			return content;
		}


	}
}

