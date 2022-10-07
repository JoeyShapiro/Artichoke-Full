using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artichoke.xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Artichoke.xamarin.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		readonly LoginViewModel _viewModel;

		public LoginPage()
		{
			InitializeComponent();
			this.BindingContext = _viewModel = new LoginViewModel();
		}

		async void btnLogin_Clicked(System.Object sender, System.EventArgs e)
		{
			if (entryGiven.Text == string.Empty || entryPassphrase.Text == string.Empty)
			{
				await DisplayAlert("Notice", "All fields must be filled in.", "ok");
				return;
			}

			(bool is_valid, Exception err) = await _viewModel.LoginVerify(entryGiven.Text, entryPassphrase.Text);
			if (err != null)
			{
				await DisplayAlert("Error", err.Message, "ok");
				return;
			}

            if (!is_valid)
			{
				await DisplayAlert("Notice", "Invalid Username or Passphrase", "ok");
				return;
			}

			// save changes for later
			Services.Settings.Save();

			//_viewModel.LoginCommand.Execute(null);
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
	}
}
