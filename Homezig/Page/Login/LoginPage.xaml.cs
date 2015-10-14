using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Connectivity.Plugin.Abstractions;
using Connectivity.Plugin;
using Acr.UserDialogs;
using Xamarin.Forms;
using Newtonsoft.Json;
using WebSocket.Portable;
using WebSocket.Portable.Interfaces;

namespace Homezig
{

	public partial class LoginPage : ContentPage
	{		
		private IProgressDialog loadingDialog;
		public static WebSocketClient ws_client;

		public LoginPage ()
		{				
			InitializeComponent ();

			CrossConnectivity.Current.ConnectivityChanged += (senderr, argss) => 
			{
				if(!argss.IsConnected)
				{
					UserDialogs.Instance.WarnToast("Connection", "No Internet Connection", 3000);
				}
				else
				{
					UserDialogs.Instance.SuccessToast("Connection", "Internet Connected", 3000);
				}

			};

		}

		async void OnChart(object sender, EventArgs args)
		{
			//var DeviceList = new testChart ();
			var DeviceList = new testChartXaml();
			//NavigationPage ew = new NavigationPage (DeviceList);

			await Navigation.PushModalAsync (DeviceList);

		}

		async void OnLogin_Clicked(object sender, EventArgs args)
		{			
			loadingDialog = UserDialogs.Instance.Loading("Connecting...",null,null,false,MaskType.Gradient);

			if (String.IsNullOrEmpty(webSocketUrl.Text))
			{
				await DisplayAlert("Validation Error", "Server URL is required", "Re-try");
			}
			else 
			{
				//if(false){
				if (String.IsNullOrEmpty (username.Text) || String.IsNullOrEmpty (password.Text)) {
					await DisplayAlert ("Validation Error", "Username and Password are required", "Re-try");
				} else {
					await App.Database.Delete_RemoteData_Item ();
					await App.Database.Delete_All_Login_Username_Show_For_Del ();
					loadingDialog.Show ();

					//System.Threading.Tasks.Task.Run (() => 
					//{
							ws_client = new WebSocketClient ();
							ws_client.Opened += websocket_Opened;
							ws_client.Closed += websocket_Closed;
							ws_client.MessageReceived += websocket_MessageReceived;		
							//ws_client.AutoSendPongResponse = true;
					//});



					try 
					{
						Debug.WriteLine ("Websocket Opening.....");
						await ws_client.OpenAsync(webSocketUrl.Text);

					} catch (Exception ex) {
						Debug.WriteLine (ex.ToString());
						Debug.WriteLine ("OpenAsync Exception");
						UserDialogs.Instance.ShowError ("Can not Connect to Websocket Server");
					}
				}

			}


		}

		async void OnLogout_Clicked(object sender, EventArgs args)
		{
			var answer = await DisplayAlert ("Logout?", "Would you like to Log it Out?", "Yes", "No");
			if(answer)
			{
				await ws_client.CloseAsync ();
			}

		}




	}
}

