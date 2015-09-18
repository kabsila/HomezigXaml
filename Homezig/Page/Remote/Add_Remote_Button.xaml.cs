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
	public partial class Add_Remote_Button : ContentPage
	{
		private IProgressDialog waitRemoteDialog;

		public Add_Remote_Button ()
		{
			InitializeComponent ();

			waitRemoteDialog = UserDialogs.Instance.Loading("Press Remote...",null,null,false,MaskType.Gradient);

			MessagingCenter.Subscribe<Add_Remote_Button> (this, "remote_code_success", (sender) => {

				//waitRemoteDialog.Hide();
				UserDialogs.Instance.ShowSuccess ("Success!");

			});
		}

		async void submitRemoteButton_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty (instructionRemoteName_value.Text)) {
				await DisplayAlert ("Validation Error", "Remote name are required", "Re-try");
			}else{

				waitRemoteDialog.Show ();
				string remoteButtonNmae = instructionRemoteName_value.Text;
				if(instructionRemoteName_value.Text.Contains(" ")){					
					remoteButtonNmae = instructionRemoteName_value.Text;
					remoteButtonNmae = remoteButtonNmae.Replace(" ", "_");
				}
				RemoteData rd = new RemoteData();
				foreach (var data in await App.Database.Get_flag_Login())
				{
					rd.remote_username = data.username;
					break;
				}
				var item = (Db_allnode)BindingContext;
				rd.ID = 0;
				rd.node_addr = item.node_addr;
				rd.remote_button_name = remoteButtonNmae;//Add_Remote_Single_Page.instructionRemoteName_value.Text;
				rd.remote_code = "single";
				rd.node_command = "add_button_remote";
				string jsonCommandaddRemoteButton = JsonConvert.SerializeObject(rd, Formatting.Indented);
				System.Diagnostics.Debug.WriteLine ("{0}",jsonCommandaddRemoteButton);
				await LoginPage.ws_client.SendAsync (jsonCommandaddRemoteButton);


				/**Device.BeginInvokeOnMainThread (() => {
					//Add_Remote_Single_Page.addRemotePageLayout.Children.Add (Add_Remote_Single_Page.plsWaitText);
					Add_Remote_Single_Page.plsWaitText.TextColor = Color.Default;
					Add_Remote_Single_Page.plsWaitText.Text = "Push remote command";
					Add_Remote_Single_Page.AddRemoteIndicator.IsRunning = true;
					Add_Remote_Single_Page.addRemoteSubmitButton.IsEnabled = false;
				});**/
			}

		}

		async void cancelRemoteButton_Click(object sender, EventArgs e)
		{

			await Navigation.PopAsync ();

		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

		}
	}
}

