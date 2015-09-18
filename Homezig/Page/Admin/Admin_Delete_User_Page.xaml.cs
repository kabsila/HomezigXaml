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
	public partial class Admin_Delete_User_Page : ContentPage
	{
		public Admin_Delete_User_Page ()
		{
			InitializeComponent ();

			usernameForDelete.ItemTapped += async (sender, e) => 
			{
				var LoginItem = (LoginUsernameForDel)e.Item;

				var answer = await DisplayAlert ("Confirm?", "Would you like to delete " + LoginItem.username, "Yes", "No");

				if (answer.Equals (true)) {
					LoginItem.node_command = "delete_user";
					string jsonCommandqueryUser = JsonConvert.SerializeObject (LoginItem, Formatting.Indented);
					System.Diagnostics.Debug.WriteLine ("userForDelete_Tapped {0}", jsonCommandqueryUser);
					await LoginPage.ws_client.SendAsync (jsonCommandqueryUser);
				} else {
					((ListView)sender).SelectedItem = null;
				}
			};
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			usernameForDelete.ItemsSource = await App.Database.Get_Login_Username_Show_For_Del();
		}
	}
}

