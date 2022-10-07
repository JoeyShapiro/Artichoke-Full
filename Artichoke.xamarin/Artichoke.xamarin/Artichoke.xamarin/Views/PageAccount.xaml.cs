using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Artichoke.xamarin.Views
{
	public partial class PageAccount : ContentPage
	{
		public string AccountFamilyId { get => Services.Settings.FamilyID; }
		public string AccountGivenId { get => Services.Settings.GivenID; }

		public PageAccount()
		{
			InitializeComponent();
		}
	}
}

