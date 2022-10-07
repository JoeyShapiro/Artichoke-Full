using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Artichoke.xamarin.Services;
using Artichoke.xamarin.Views;
using Artichoke.xamarin.Models;
using Xamarin.Essentials;

namespace Artichoke.xamarin
{
    public partial class App : Application
    {

        public App ()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            Settings.Load();

            MainPage = new AppShell();
            var start_path =
                Settings.GivenID == null ? $"//{nameof(LoginPage)}" : $"//{nameof(ItemsPage)}";

            Shell.Current.GoToAsync(start_path);
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

