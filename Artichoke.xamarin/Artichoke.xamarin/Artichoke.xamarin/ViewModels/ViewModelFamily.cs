using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Artichoke.xamarin.Models;
using Xamarin.Forms;
using Artichoke.xamarin.Services;
using System.Linq;

namespace Artichoke.xamarin.ViewModels
{
	public class ViewModelFamily : BaseViewModel
	{
		//private ObservableFamily myFamily;
		//public ObservableFamily MyFamily { get { return myFamily; } set { SetProperty(ref myFamily, value); OnPropertyChanged("MyFamily"); } }
		public ObservableCollection<Family> MyFamily { get; set; }

		public Command LoadFamilyCommand { get; }
		public string result = "hello";
		public string Result { get { return result; } set { SetProperty(ref result, value); OnPropertyChanged("Result"); } }

		public ViewModelFamily()
		{
			Title = "My Family";
			MyFamily = new ObservableCollection<Family>();
			LoadFamilyCommand = new Command(async () => await ExecuteLoadFamilyCommand());
		}

		private async Task ExecuteLoadFamilyCommand()
		{
			IsBusy = true;

			try
			{
				var myFamily = await API_Interface.GetMyFamily();
				myFamily.SubExpiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(myFamily.SubExpirationUnix)).UtcDateTime.ToString("U"); //MMM d, yyyy @ T
				MyFamily.Add(myFamily);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}
	}
}

