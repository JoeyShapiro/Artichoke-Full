using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Artichoke.xamarin.Services;
using Artichoke.xamarin.Views;
using Artichoke.xamarin.Models;

namespace Artichoke.xamarin
{
    public partial class App : Application
    {

        public App ()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            Account.Load();
            MainPage = new AppShell();
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

