using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Artichoke.xamarin.Models;
using Artichoke.xamarin.Views;
using Artichoke.xamarin.ViewModels;

namespace Artichoke.xamarin.Views
{	
	public partial class PageFamily : ContentPage
	{
        ViewModelFamily _viewModelFamily;

		public PageFamily ()
		{
			InitializeComponent ();

            BindingContext = _viewModelFamily = new ViewModelFamily();
            Console.WriteLine("hello");
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("hello");

            _viewModelFamily.Result = await _viewModelFamily.RefreshDataAsync();
            await DisplayAlert("test", _viewModelFamily.Result, "cancel");
        }
    }
}

