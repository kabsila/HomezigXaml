using System;
using System.Collections.Generic;
using SQLite.Net;
using SQLite.Net.Async;
using Newtonsoft.Json;
using WebSocket.Portable;
using WebSocket.Portable.Interfaces;
using Xamarin.Forms;

namespace Homezig
{
	public partial class Change_Password_Page : ContentPage
	{
		public Change_Password_Page ()
		{
			InitializeComponent ();
		}

		public async void OnSubmit_Clicked (object sender, EventArgs e) 
		{
			if (String.IsNullOrEmpty (newPassword.Text)) {
				await DisplayAlert ("Validation Error", "New Password are required", "Re-try");
			} else {

				string jsonCommandChangePassword = "";
				foreach (var data in await App.Database.Get_flag_Login())  
				{
					data.password = Sha256.sha256_hash (newPassword.Text);
					data.node_command = "change_user_password";
					jsonCommandChangePassword = JsonConvert.SerializeObject (data, Formatting.Indented);
					break;
				}
				System.Diagnostics.Debug.WriteLine ("jsonCommandChangePassword {0}", jsonCommandChangePassword);
				await LoginPage.ws_client.SendAsync (jsonCommandChangePassword);
			}
			
		}

		async void OnCancel_Clicked (object sender, EventArgs e) 
		{
			await Navigation.PopAsync();
		}
	}
}

