using System.ComponentModel;
using Xamarin.Forms;
using Artichoke.xamarin.ViewModels;

namespace Artichoke.xamarin.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
