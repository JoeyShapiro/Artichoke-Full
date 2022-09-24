using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Artichoke.xamarin.Models;
using Artichoke.xamarin.Views;
using Artichoke.xamarin.ViewModels;
using Xamarin.Forms.PlatformConfiguration;

namespace Artichoke.xamarin.Views
{
	public partial class ItemsPage : ContentPage
	{
		ItemsViewModel _viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ItemsViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.OnAppearing();
		}

		async void ToggleAdd_Clicked(System.Object sender, System.EventArgs e)
		{
			if (!addDiv.IsVisible)
			{
				await addDiv.TranslateTo(App.Current.MainPage.Width, 0, 0);
				addDiv.IsVisible = true;
				await addDiv.TranslateTo(0, 0, 200);
				addButton.Text = "Cancel";
			}
			else
			{
				await addDiv.TranslateTo(App.Current.MainPage.Width, 0, 200);
				addDiv.IsVisible = false;
				addButton.Text = "Add";
			}
		}

		async void addSave_Clicked(System.Object sender, System.EventArgs e)
		{
			if (addPicker.SelectedIndex < 0)
				return;

			string name = addEntry.Text;
			string desc = addDesc.Text;
			string category = addPicker.Items[addPicker.SelectedIndex];

			if (name == string.Empty || desc == string.Empty || category == string.Empty)
				return;

			Item item = new Item { Name = name, Desc = desc, Category = category };

			Exception err = await _viewModel.AddItem(item);

			if (err != null)
				await DisplayAlert("Error", err.Message, "ok");
			else
			{
				addEntry.Text = string.Empty;
				addDesc.Text = string.Empty;
				addPicker.SelectedIndex = -1;
			}
		}
	}
}
