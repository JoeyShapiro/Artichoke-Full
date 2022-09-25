using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Artichoke.xamarin.Models;
using Artichoke.xamarin.Services;
using Xamarin.Forms;

namespace Artichoke.xamarin.ViewModels
{
	public class ViewModelLogs : BaseViewModel
	{
		public ObservableCollection<Log> Logs { get; set; }
		public Command LoadLogsCommand { get; }


		public ViewModelLogs()
		{
			Title = "Family Logs";
			Logs = new ObservableCollection<Log>();
			LoadLogsCommand = new Command(async () => await ExecuteLoadLogsCommand());
		}

		private async Task ExecuteLoadLogsCommand()
		{
			IsBusy = true;

			try
			{
				(var logs, Exception err) = await API_Interface.GetFamilyLogs();
				if (err != null)
                    await Application.Current.MainPage.DisplayAlert("Error", err.Message, "ok");

				foreach (Log log in logs)
					Logs.Add(log);
            }
			catch (Exception ex)
			{
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
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

