using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Artichoke.xamarin.Models;
using Artichoke.xamarin.Views;
using Artichoke.xamarin.Services;
using System.Linq;
using System.Collections.Generic;

namespace Artichoke.xamarin.ViewModels
{
	public class ItemsViewModel : BaseViewModel
	{
		private Item _selectedItem;

		public ObservableCollection<ItemGroup> Items { get; }
		public ObservableCollection<string> Categories { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get;  }
		public Command<Item> ItemTapped { get; }
		public Command<Item> ItemSwiped { get; }

		public ItemsViewModel()
		{
			Title = "Browse";

			Items = new ObservableCollection<ItemGroup>();
			Categories = new ObservableCollection<string>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

			ItemTapped = new Command<Item>(OnItemSelected);
			ItemSwiped = new Command<Item>(OnItemSwiped);

			AddItemCommand = new Command(OnAddItem);
		}

		async Task ExecuteLoadItemsCommand()
		{
			IsBusy = true;

			try
			{
				Items.Clear();
				Categories.Clear();

				(var categories, Exception err) = await API_Interface.GetCategories();
				if (err != null)
                    await Application.Current.MainPage.DisplayAlert("Error", err.Message, "ok"); //TODO should this also return
                (var items_left, Exception err2) = await API_Interface.GetItemsLeft();
                if (err2 != null)
                    await Application.Current.MainPage.DisplayAlert("Error", err.Message, "ok");
                (var items_collected, Exception err3) = await API_Interface.GetItemsCollected();
                if (err3 != null)
                    await Application.Current.MainPage.DisplayAlert("Error", err.Message, "ok");

                var itemGroupsLeft = new Dictionary<string, ItemGroup>();
				foreach(var category in categories)
				{
					itemGroupsLeft.Add(category.Name, new ItemGroup(category.Name, new List<Item>()));
					Categories.Add(category.Name);
				}

				foreach(var item in items_left)
				{
					itemGroupsLeft[item.Category].Add(item);
				}

				foreach(var itemGroup in itemGroupsLeft)
					Items.Add(itemGroup.Value);
				Items.Add(new ItemGroup("Recently Collected", items_collected.ToList()));
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
			SelectedItem = null;
		}

		public Item SelectedItem
		{
			get => _selectedItem;
			set
			{
				SetProperty(ref _selectedItem, value);
				OnItemSelected(value);
			}
		}

		private async void OnAddItem(object obj)
		{
			//await API_Interface.ItemAdd();
            //await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

		async void OnItemSelected(Item item)
		{
			if (item == null)
				return;

			// This will push the ItemDetailPage onto the navigation stack
			await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
		}

		private async void OnItemSwiped(Item item)
		{
			if (item == null)
				return;

			Exception err = await API_Interface.ItemCollect(item);
			if (err != null)
			{
				await ExecuteLoadItemsCommand();
				await Application.Current.MainPage.DisplayAlert("Error", err.Message, "ok");
			}
			else
				await ExecuteLoadItemsCommand();


		}

		public async Task<Exception> AddItem(Item item)
		{
			Exception err = await API_Interface.ItemAdd(item);

            await ExecuteLoadItemsCommand();

			return err; // this will print error after
		}
	}
}
