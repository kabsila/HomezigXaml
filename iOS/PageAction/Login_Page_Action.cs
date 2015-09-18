using System;
using Xamarin.Forms;
using Homezig.iOS;
using WebSocket4Net;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Timers;
using Toasts.Forms.Plugin.Abstractions;


[assembly: Dependency (typeof (Login_Page_Action))]
namespace Homezig.iOS
{
	
	public class Login_Page_Action : ContentPage, I_Login
	{
		//public static IPageManager ipm1;
		public static Timer tmr ;
		public static WebsocketManager nws = null;

		public Login_Page_Action ()
		{
		}

		public async void LoginButtonClick(object sender, EventArgs e)
		{
			LoginPage.loginFail.IsVisible = false;
			if (String.IsNullOrEmpty(LoginPage.username.Text) || String.IsNullOrEmpty(LoginPage.password.Text))
			{
				await DisplayAlert("Validation Error", "Username and Password are required", "Re-try");
			} else {

				//code here for check valid username and password from server
				new System.Threading.Thread (new System.Threading.ThreadStart (() => {
					Device.BeginInvokeOnMainThread (() => {
						LoginPage.loginButton.IsEnabled = false;
						LoginPage.activityIndicator.IsRunning = true;
					});
				})).Start();

				Login loginData = new Login ();
				loginData.lastConnectWebscoketUrl = LoginPage.websocketUrl.Text;
				loginData.username = LoginPage.username.Text;
				loginData.password = sha256_hash(LoginPage.password.Text);
				loginData.flagForLogin = "";
				loginData.node_command = "check_login";

				//no await App.Database.Save_Login_Item (loginData.username, LoginPage.password.Text, "pass", loginData.lastConnectWebscoketUrl);

				string jsonCommandLogin = JsonConvert.SerializeObject(loginData, Formatting.Indented);
				System.Diagnostics.Debug.WriteLine ("jsonCommandLogin" , jsonCommandLogin);
				WebsocketManager.websocketMaster.Send (jsonCommandLogin);
				//WebsocketManager.websocketMaster.Send("{\"cmd_login\":[{\"username\":\"admin\",\"flagForLogin\":\"pass\",\"lastConnectWebscoketUrl\":\"ws://echo.websocket.org\"}]}");
				// no ipm1.showMenuTabPage ();
			}
		}

		public async void LogoutButtonClick(object sender, EventArgs e)
		{			
			await App.Database.Delete_Login_Item ();
			await App.Database.Delete_RemoteData_Item();
			await App.Database.Delete_All_Login_Username_Show_For_Del ();

			try{
				WebsocketManager.websocketMaster.Close ();
			}catch{
				System.Diagnostics.Debug.WriteLine ("[LogoutButtonClick] Logout when websocket not connected");
			}

			new System.Threading.Thread (new System.Threading.ThreadStart (() => {
				Device.BeginInvokeOnMainThread (() => {
					LoginPage.username.Text = "";
					LoginPage.password.Text = "";
					LoginPage.username.IsEnabled = true;
					LoginPage.password.IsEnabled = true;
					LoginPage.ConnectButton.IsEnabled = true;
					LoginPage.loginButton.IsEnabled = true;
					LoginPage.logoutButton.IsEnabled = false;
					LoginPage.activityIndicator.IsRunning = false;
				});
			})).Start();


			//LoginPage.loginButton.IsEnabled = true;
		}

		public async void ConnectButton_Click(object sender, EventArgs e)
		{
			

			if (String.IsNullOrEmpty(LoginPage.websocketUrl.Text))
			{
				await DisplayAlert("Validation Error", "Server URL is required", "Re-try");
			} else {
				tmr = new Timer();
				tmr.Interval = 30000; // 10 second
				tmr.Elapsed += timerHandler; // We'll write it in a bit
				tmr.Start();


				//if (nws == null) 
				//{				
					nws = new WebsocketManager ();
					System.Diagnostics.Debug.WriteLine ("1111111111111111");
				//}

				LoginPage.loginButton.IsEnabled = false;
				LoginPage.ConnectButton.IsEnabled = false;
				LoginPage.activityIndicator.IsRunning = true;

				try
				{		
					System.Diagnostics.Debug.WriteLine ("2222222222222222222222");					
					WebsocketManager.websocketMaster.Open ();
				}
				catch
				{
					System.Diagnostics.Debug.WriteLine ("33333333333333333333");
					tmr.Stop ();
					//tmr.Dispose ();
					LoginPage.activityIndicator.IsRunning = false;
					LoginPage.ConnectButton.IsEnabled = true;
				}

			}
		}

		public static String sha256_hash(String value) {
			StringBuilder Sb = new StringBuilder();

			using (SHA256 hash = SHA256Managed.Create()) {
				Encoding enc = Encoding.UTF8;
				Byte[] result = hash.ComputeHash(enc.GetBytes(value));

				foreach (Byte b in result)
					Sb.Append(b.ToString("x2"));
			}

			return Sb.ToString();
		}

		private void timerHandler(object sender, EventArgs e) {
			WebsocketManager.websocketMaster.Dispose ();
			new System.Threading.Thread (new System.Threading.ThreadStart (() => {
				Device.BeginInvokeOnMainThread (async () => {
					var notificator = DependencyService.Get<IToastNotificator>();
					await notificator.Notify(ToastNotificationType.Warning, 
						"TimeOut", "Check your internet connection", TimeSpan.FromSeconds(3));
					LoginPage.ConnectButton.IsEnabled = true;
					LoginPage.activityIndicator.IsRunning = false;
				});
			})).Start();



			tmr.Stop(); // Manually stop timer, or let run indefinitely
		}
	}
}

