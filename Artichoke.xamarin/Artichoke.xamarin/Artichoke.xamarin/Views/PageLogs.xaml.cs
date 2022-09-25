using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Artichoke.xamarin.ViewModels;

namespace Artichoke.xamarin.Views
{
    public partial class PageLogs : ContentPage
    {
        ViewModelLogs _viewModelLogs;

        public PageLogs()
        {
            InitializeComponent();

            BindingContext = _viewModelLogs = new ViewModelLogs();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModelLogs.OnAppearing();
        }
    }
}

