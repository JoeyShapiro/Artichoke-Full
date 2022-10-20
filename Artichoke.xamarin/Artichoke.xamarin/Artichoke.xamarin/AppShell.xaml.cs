using System;
using System.Collections.Generic;
using Artichoke.xamarin.Models;
using Artichoke.xamarin.ViewModels;
using Artichoke.xamarin.Views;
using Xamarin.Forms;

namespace Artichoke.xamarin
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");

            //TODO this should be handled by login page, or the class
            // remove settings *after* logout (will update ui)
            Services.Settings.GivenID = "";
            Services.Settings.GivenPassphraseHash = "";
            Services.Settings.FamilyID = "";
            Services.Settings.FamilyPassphraseHash = "";
            Services.Settings.Save();
        }
    }
}

