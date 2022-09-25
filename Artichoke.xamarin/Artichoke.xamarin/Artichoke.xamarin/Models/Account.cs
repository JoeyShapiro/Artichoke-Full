using System;
using Xamarin.Forms;

namespace Artichoke.xamarin.Models
{
	public static class Account
	{
		static string family_id = "";
		static string given_id = "";
		static string family_passphrase_hash = "";

		public static string FamilyID { get => family_id; set => family_id = value; }
		public static string GivenID { get => given_id; set => given_id = value; }
		public static string FamilyPassphraseHash { get => family_passphrase_hash; set => family_passphrase_hash = value; }

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
		}
	}
}