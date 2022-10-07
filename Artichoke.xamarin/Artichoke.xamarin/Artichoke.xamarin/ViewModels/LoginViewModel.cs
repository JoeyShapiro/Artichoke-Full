using Artichoke.xamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Artichoke.xamarin.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		public Command LoginCommand { get; }

		public LoginViewModel()
		{
			LoginCommand = new Command(OnLogin);
		}

		private async void OnLogin(object obj)
		{
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			//await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
		}

		public async Task<(bool, Exception)> LoginVerify(string given_id, string given_passphrase_hash)
		{
			// verity
			(var account, Exception err) = await Services.API_Interface.GetAccount(given_id, given_passphrase_hash);
			if (err != null)
				return (false, err);

			// store in settings
			Services.Settings.GivenID = account.GivenId;
			Services.Settings.GivenPassphraseHash = account.GivenPassphraseHash;
			Services.Settings.FamilyID = account.FamilyId;
			Services.Settings.FamilyPassphraseHash = account.FamilyPassphraseHash;

			// return true
			return (true, null);
		}
	}
}

