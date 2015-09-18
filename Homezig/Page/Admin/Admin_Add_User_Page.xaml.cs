using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using SQLite.Net;
using SQLite.Net.Async;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Homezig
{
	public partial class Admin_Add_User_Page : ContentPage
	{
		public Admin_Add_User_Page ()
		{
			InitializeComponent ();
		}

		async void OnCreateUser(object sender, EventArgs args)
		{
			if (String.IsNullOrEmpty (usernameEntry.Text) || String.IsNullOrEmpty (passwordEntry.Text)) {
				await DisplayAlert ("Validation Error", "Username and Password are required", "Re-try");
			} else {
				Login loginData = new Login ();
				loginData.lastConnectWebscoketUrl = "";
				loginData.username = usernameEntry.Text;
				loginData.password = Sha256.sha256_hash (passwordEntry.Text);
				loginData.flagForLogin = "";
				loginData.node_command = "add_user";				 

				string jsonCommandLogin = JsonConvert.SerializeObject (loginData, Formatting.Indented);
				System.Diagnostics.Debug.WriteLine ("jsonCommandLogin {0}", jsonCommandLogin);
				await LoginPage.ws_client.SendAsync (jsonCommandLogin);

			}
		}

		async void OnCancel(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
	}
}

