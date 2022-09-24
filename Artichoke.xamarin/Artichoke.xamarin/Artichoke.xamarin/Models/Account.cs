using System;
namespace Artichoke.xamarin.Models
{
	public static class Account
	{
		static string family_id;
		static string given_id;
		static string family_passphrase_hash;

		public static string FamilyID { get => family_id; set => family_id = value; }
		public static string GivenID { get => given_id; set => given_id = value; }
		public static string FamilyPassphraseHash { get => family_passphrase_hash; set => family_passphrase_hash = value; }
	}
}

