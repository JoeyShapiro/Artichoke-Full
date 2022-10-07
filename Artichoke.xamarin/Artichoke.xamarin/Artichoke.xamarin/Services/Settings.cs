using System;
using Xamarin.Forms;

namespace Artichoke.xamarin.Services
{
	public static class Settings
	{
		//TODO maybe give account object. or make dictionary, for easy access
		public static string GivenID { get; set; }
		public static string GivenPassphraseHash { get; set; }
		public static string FamilyID { get; set; }
		public static string FamilyPassphraseHash { get; set; }

		public static void Load()
		{
			if (Application.Current.Properties.ContainsKey("family_id"))
				FamilyID = Application.Current.Properties["family_id"] as string;
			if (Application.Current.Properties.ContainsKey("given_id"))
				GivenID = Application.Current.Properties["given_id"] as string;
			if (Application.Current.Properties.ContainsKey("family_passphrase_hash"))
				FamilyPassphraseHash = Application.Current.Properties["family_passphrase_hash"] as string;
		}

		public static void Save()
		{
			Application.Current.Properties["family_id"] = FamilyID;
			Application.Current.Properties["given_id"] = GivenID;
			Application.Current.Properties["family_passphrase_hash"] = FamilyPassphraseHash;

			Application.Current.SavePropertiesAsync();
		}
	}
}

