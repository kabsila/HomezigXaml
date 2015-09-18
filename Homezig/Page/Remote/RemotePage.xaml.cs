using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite.Net;
using SQLite.Net.Async;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Acr.UserDialogs;
namespace Homezig
{
	public partial class RemotePage : ContentPage
	{
		private IProgressDialog waitRemoteDialog;

		public RemotePage ()
		{
			InitializeComponent ();



			MessagingCenter.Subscribe<RemotePage> (this, "delete_remote_success", async(sender) => {

				UserDialogs.Instance.ShowSuccess ("Success!");
				remoteButtonListName.BindingContext = await App.Database.Get_RemoteData_Item();

			});

			MessagingCenter.Subscribe<RemotePage> (this, "rename_remote_success", (sender) => {

				UserDialogs.Instance.ShowSuccess ("Success!");
				Device.BeginInvokeOnMainThread( async ()=>
				{
						remoteButtonListName.BindingContext = await App.Database.Get_RemoteData_Item();
				});


			});



		}

		public async void RemoteControl_Tapped(object sender, ItemTappedEventArgs e)
		{
			var irCommand = (RemoteData)e.Item;
			foreach (var data in await App.Database.Get_flag_Login())
			{
				irCommand.remote_username = data.username;
				irCommand.node_command = "ir_remote_command";
				string jsonirCommand = JsonConvert.SerializeObject(irCommand, Formatting.Indented);
				System.Diagnostics.Debug.WriteLine (jsonirCommand);
				await LoginPage.ws_client.SendAsync (jsonirCommand);
				break;
			}
			((ListView)sender).SelectedItem = null;

		}

		async void OnRename(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			var remote = (RemoteData)mi.BindingContext;

			var result = await UserDialogs.Instance.PromptAsync(new PromptConfig {
				Title = "Rename",
				Text = remote.remote_button_name,
				IsCancellable = true,
				Placeholder = "Type new name"

			});

			if(!result.Text.Equals(remote.new_button_name)){

				waitRemoteDialog = UserDialogs.Instance.Loading("Renaming...",null,null,false,MaskType.Gradient);
				waitRemoteDialog.Show ();

				var newName = result.Text;
				RemoteData itemRemote =  new RemoteData();
				itemRemote.remote_button_name = remote.remote_button_name;
				itemRemote.node_command = "ir_remote_rename";
				itemRemote.new_button_name = newName;
				itemRemote.node_addr = remote.node_addr;
				itemRemote.remote_username = remote.remote_username;
				string jsonCommandaddRemoteButton = JsonConvert.SerializeObject(itemRemote, Formatting.Indented);
				System.Diagnostics.Debug.WriteLine ("{0}",jsonCommandaddRemoteButton);
				await LoginPage.ws_client.SendAsync (jsonCommandaddRemoteButton);

			}

			System.Diagnostics.Debug.WriteLine("RenameRemote_Clicked");
		}

		async void OnDelete(object sender, EventArgs e)
		{
			
			var mi = ((MenuItem)sender);
			var remoteData = (RemoteData)mi.BindingContext;

			var answer = await DisplayAlert ("Confirm?", "Would you like to delete " + remoteData.remote_button_name, "Yes", "No");

			if (answer.Equals (true)) {
				
				waitRemoteDialog = UserDialogs.Instance.Loading("Deleteting...",null,null,false,MaskType.Gradient);
				waitRemoteDialog.Show ();

				remoteData.remote_button_name = remoteData.remote_button_name;
				remoteData.node_command = "delete_button_remote";
				string jsonCommandaddRemoteButton = JsonConvert.SerializeObject(remoteData, Formatting.Indented);
				System.Diagnostics.Debug.WriteLine ("{0}",jsonCommandaddRemoteButton);
				await LoginPage.ws_client.SendAsync (jsonCommandaddRemoteButton);

			} else {

			}
		}

		void onAddButtonClick(object sender, EventArgs e)
		{
			var DeviceList = new Add_Remote_Button ();
			DeviceList.BindingContext = (Db_allnode)BindingContext;
			Navigation.PushAsync (DeviceList);
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			//var item = (Db_allnode)BindingContext;
			remoteButtonListName.BindingContext = await App.Database.Get_RemoteData_Item();
		}
	}
}

