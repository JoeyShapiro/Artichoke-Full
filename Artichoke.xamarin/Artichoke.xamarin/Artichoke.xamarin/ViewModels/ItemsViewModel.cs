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
		public ObservableCollection<Category> Categories { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get;  }
		public Command<Item> ItemTapped { get; }
		public Command<Item> ItemSwiped { get; }
		public Category SelectedCategory { get; set; }

		public ItemsViewModel()
		{
			Title = "Browse";

			Items = new ObservableCollection<ItemGroup>();
			Categories = new ObservableCollection<Category>();
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
				await reload();
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

		//? not sure why this has a task, but it does
		private async Task reload()
		{
			//         Items.Clear();
			//Categories.Clear();
			//while (Items.Count > 0) // this seems faster than clear
			//	Items.RemoveAt(0);

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
			foreach (var category in categories)
			{
				itemGroupsLeft.Add(category.Name, new ItemGroup(category.Name, new List<Item>()));
				Categories.Add(category);
			}

			foreach (var item in items_left)
			{
				itemGroupsLeft[item.Category].Add(item);
				//if (Items.Single(group => group.Name == item.Category).Contains(item))

			}

			if (Items.Count > 0)
			{
                #region Brian's Code
    //            for (int i = 0; i < Items.Count; i++)
				//{
				//	if (i < Items.Count - 1)
				//		Items[i] = itemGroupsLeft[Items[i].Name];
				//	else
				//		Items[i] = new ItemGroup("Recently Collected", items_collected.ToList());
    //            }
				#endregion

				#region Joey's Redemption
				var itemsNew = new ObservableCollection<ItemGroup>();

                foreach (var itemGroup in itemGroupsLeft)
                    if (itemGroup.Value.Count > 0)
                    {
                        await Task.Delay(1); // this needs to be here for ios to load an observable collection
                        itemsNew.Add(itemGroup.Value);
						// they could be misallined
                    }
				Items[Items.Count - 1] = new ItemGroup("Recently Collected", items_collected.ToList());
                #endregion

                #region Joey's Code
                //// remove needed items
                //for (int j = 0; j < Items.Count; j++)
                //{
                //	var itemGroup = Items[j];

                //	for (int i = itemGroup.Count - 1; i >= 0; i--)
                //	{
                //		var itemOld = itemGroup[i];

                //		if (itemGroup.Name == "Recently Collected")
                //		{
                //			var groupContains = items_collected.SingleOrDefault(item => item.Id == itemOld.Id);

                //			if (groupContains == null)
                //			{
                //				itemGroup.Remove(itemOld);
                //			}
                //		}
                //		else
                //		{
                //			var groupNew = itemGroupsLeft[itemGroup.Name];
                //			var groupContains = groupNew.SingleOrDefault(item => item.Id == itemOld.Id);

                //			if (groupContains == null)
                //			{
                //				itemGroup.Remove(itemOld);
                //			}
                //		}
                //	}

                //	Items[j] = itemGroup; // arbitrary reset so change can be seen
                //}
                //// add needed items
                //// items left
                //foreach (var itemNew in items_left)
                //{
                //	var foundItem = false;

                //	var foundGroup = Items.SingleOrDefault(groupOld => groupOld.Name == itemNew.Category);
                //	if (foundGroup != null)
                //		foundItem = foundGroup.SingleOrDefault(item => item.Id == itemNew.Id) != null;

                //	if (!foundItem && foundGroup != null)
                //	{
                //		foundGroup.Add(itemNew);
                //		for (int i = 0; i < Items.Count; i++)
                //			if (Items[i].Name == foundGroup.Name)
                //				Items[i] = foundGroup;
                //	}
                //	else if (!foundItem && foundGroup == null)
                //	{
                //		var list = new List<Item>();
                //		list.Add(itemNew);
                //		Items.Add(new ItemGroup(itemNew.Category, list));
                //	}
                //}
                //// items collected
                //foreach (var itemNew in items_collected)
                //{
                //	var foundItem = false;
                //	var foundGroup = Items.SingleOrDefault(groupOld => groupOld.Name == "Recently Collected");
                //	foundItem = foundGroup.SingleOrDefault(item => item.Id == itemNew.Id) != null;

                //	if (!foundItem)
                //	{
                //		foundGroup.Insert(0, itemNew); // need to add and remove, or something
                //		for (int i = 0; i < Items.Count; i++)
                //			if (Items[i].Name == foundGroup.Name)
                //				Items[i] = foundGroup;
                //	}
                //}
                #endregion
            }
            else
			{
				foreach (var itemGroup in itemGroupsLeft)
					if (itemGroup.Value.Count > 0)
					{
						await Task.Delay(1); // this needs to be here for ios to load an observable collection
						Items.Add(itemGroup.Value);
					}
				Items.Add(new ItemGroup("Recently Collected", items_collected.ToList()));
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
				//await ExecuteLoadItemsCommand();
				await Application.Current.MainPage.DisplayAlert("Error", err.Message, "ok");
			}
			//else
			//await ExecuteLoadItemsCommand();

			await reload(); // you cant call the load function, maybe the `isbusy` thing
		}

		public async Task<Exception> AddItem(Item item)
		{
			Exception err = await API_Interface.ItemAdd(item, SelectedCategory.id);

			await reload();

			return err; // this will print error after
		}
	}
}
