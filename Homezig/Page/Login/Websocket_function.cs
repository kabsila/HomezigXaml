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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Homezig
{
	public partial class LoginPage : ContentPage
	{
		


		private async void websocket_Closed()
		{
			Debug.WriteLine("websocket_Closed");
			await Navigation.PopModalAsync ();

		}

		private async void websocket_Opened()
		{			
			Debug.WriteLine("websocket_Opened");
			#region testws1
			await ws_client.SendAsync("{\"cmd_login\":[{\"username\":\"admin\",\"flagForLogin\":\"pass\",\"lastConnectWebscoketUrl\":\"ws://echo.websocket.org\"}]}");
			#endregion

			//LoginPage.loginFail.IsVisible = false;
			if (String.IsNullOrEmpty(username.Text) || String.IsNullOrEmpty(password.Text))
			{
				await DisplayAlert("Validation Error", "Username and Password are required", "Re-try");
			} else {

				//code here for check valid username and password from server
				/**new System.Threading.Thread (new System.Threading.ThreadStart (() => {
					Device.BeginInvokeOnMainThread (() => {
						LoginPage.loginButton.IsEnabled = false;
						LoginPage.activityIndicator.IsRunning = true;
					});
				})).Start();**/

				Login loginData = new Login ();
				loginData.lastConnectWebscoketUrl = webSocketUrl.Text;
				loginData.username = username.Text;
				loginData.password = Sha256.sha256_hash(password.Text);
				loginData.flagForLogin = "";
				loginData.node_command = "check_login";

				//no await App.Database.Save_Login_Item (loginData.username, LoginPage.password.Text, "pass", loginData.lastConnectWebscoketUrl);

				string jsonCommandLogin = JsonConvert.SerializeObject(loginData, Formatting.Indented);
				System.Diagnostics.Debug.WriteLine ("jsonCommandLogin" , jsonCommandLogin);
				await ws_client.SendAsync (jsonCommandLogin);
				//WebsocketManager.websocketMaster.Send (jsonCommandLogin);
				//WebsocketManager.websocketMaster.Send("{\"cmd_login\":[{\"username\":\"admin\",\"flagForLogin\":\"pass\",\"lastConnectWebscoketUrl\":\"ws://echo.websocket.org\"}]}");
				// no ipm1.showMenuTabPage ();
			}
		}

		private async void websocket_MessageReceived(IWebSocketMessage e)
		{
			
			Debug.WriteLine ("websocket_MessageReceived \n {0}", e.ToString());

			try
			{
				RootObject cmd = JsonConvert.DeserializeObject<RootObject>(e.ToString());

				if(cmd.cmd_db_allnode != null){

					Debug.WriteLine("node_change_detected");
					foreach(var data in cmd.cmd_db_allnode)
					{
						bool isUpdate = false;
						try
						{
							await App.Database.Save_DBAllNode_Item(data);							
						}
						catch (Exception exx)
						{
							isUpdate = true;
							Debug.WriteLine("Exception {0}", exx.ToString());
						}

						if (isUpdate)
						{
							await App.Database.Update_DBAllNode_All_Item(data);
						}
					}

					foreach(var data in await App.Database.GetItems())
					{
						#region io_value

						#endregion
						var ioNUmber = 0;
						if(data.node_deviceType.Equals(EnumtoString.EnumString(NodeDeviceType.InWallSwitch))){
							ioNUmber = 2;
						}else if(data.node_deviceType.Equals(EnumtoString.EnumString(NodeDeviceType.GeneralPurposeDetector))){
							ioNUmber = 4;
						}else{
							ioNUmber = 1;
						}

						try
						{							
							for(var i = 0;i < ioNUmber;i++)
							{
								string io_state = find_io_value(i, data.node_io);
								await App.Database.Update_NameByUser_ioValue2(data.node_io, io_state, data.node_addr, i.ToString());
								if(!data.node_deviceType.Equals(EnumtoString.EnumString(NodeDeviceType.UnknowDeviceType))){
									await App.Database.Save_NameByUser(data, i.ToString(), i.ToString(), io_state);
								}

							}
						}
						catch (Exception exx)
						{
							Debug.WriteLine("Exception2 {0}", exx.ToString());
						}
					}
					foreach(var i in await App.Database.GetItems())
					{
						//Log.Info ("From Database" , i.node_addr);
						//Log.Info ("From node_status" , i.node_status);
					}

					foreach(var i in await App.Database.Get_NameByUser())
					{
						//System.Diagnostics.Debug.WriteLine("=====> {0}, {1}, {2}", i.node_addr, i.io_name_by_user, i.target_io);
						//Log.Info ("From Get_NameByUser" , String.Format("=====> {0}, ->{1}<-, {2}, {3}, {4}, {5}", i.node_addr, i.node_name_by_user, i.node_io, i.io_name_by_user, i.node_io_p, i.io_value));
						Debug.WriteLine("=====> {0}, ->{1}<-, {2}, {3}, {4}, {5}", i.node_addr, i.node_name_by_user, i.node_io, i.io_name_by_user, i.node_io_p, i.io_value);
						//Log.Info ("From Get_NameByUser" , i.node_name_by_user);
						//Log.Info ("From Get_NameByUser" , i.io_name_by_user);
						//Log.Info ("From Get_NameByUser" , i.target_io);
					}

					var node_command = cmd.cmd_db_allnode[0].node_command;

					if(node_command.Equals("io_change_detected")){
						
						if(cmd.cmd_db_allnode[0].node_deviceType.Equals(EnumtoString.EnumString(NodeDeviceType.InWallSwitch))){
							Debug.WriteLine ("io_change_detectedkk" , "kk");
							MessagingCenter.Send<Node_io_ItemPage, string> (new Node_io_ItemPage(), "Node_io_ItemPage_Io_Change", cmd.cmd_db_allnode[0].node_addr);

							/**new System.Threading.Tasks.Task (() => {
								Device.BeginInvokeOnMainThread ( async () => {									

								});
							}).Start();**/

						}else if(cmd.cmd_db_allnode[0].node_deviceType.Equals(EnumtoString.EnumString(NodeDeviceType.GeneralPurposeDetector))){

							Debug.WriteLine ("GPD_io_Change");

							MessagingCenter.Send<Node_io_GpdPage, string> (new Node_io_GpdPage(), "Node_io_GpdPage_Io_Change", cmd.cmd_db_allnode[0].node_addr);

							/**var profileStatus = await App.Database.Get_Profile_Name_Is_Open("True");

							foreach(var data in profileStatus)
							{
								foreach(var item in await App.Database.Get_Profile_IO_Data_By_Addr(cmd.cmd_db_allnode[0].node_addr, data.profileName))
								{										
									if(item.alert_mode.Equals("True"))
									{
										foreach(var nodeName in await App.Database.Get_NameByUser_by_addr(item.node_addr))
										{
											showNotify(nodeName.node_name_by_user, item.node_addr);
											break;
										}
									}
									break;
								}
							}**/

						}else{
							
							Debug.WriteLine ("aaaaaaaaaaa");
						}

					}else if(node_command.Equals("db_allnode")){

						Debug.WriteLine("MessageReceived4  db_allnode");
						loadingDialog.Hide();
						//App.current.MainPage = new MenuTabPage();
						await Navigation.PushModalAsync(new MenuTabPage());

					}

				}else if(cmd.cmd_login != null){

					#region cmd_login
					Debug.WriteLine("cmd_login");
					#endregion

					foreach(var data in cmd.cmd_login)
					{
						if(data.flagForLogin.Equals("pass") && data.username.Equals(username.Text)){
							#region testws2
							await ws_client.SendAsync ("{\"cmd_db_allnode\":[{\"node_type\":\"0x3ff20\",\"node_addr\":\"[00:13:a2:00:40:ad:58:rm]!\",\"node_status\":\"0\",\"node_io\":\"FC\",\"node_command\":\"db_allnode\"},{\"node_type\":\"0x3ff01\",\"node_addr\":\"[00:13:a2:00:40:ad:58:ab]!\",\"node_status\":\"0\",\"node_io\":\"FE\",\"node_command\":\"db_allnode\"}, {\"node_type\":\"0x3ff01\",\"node_addr\":\"[00:13:a2:00:40:ad:58:ae]!\",\"node_status\":\"0\",\"node_io\":\"FE\",\"node_command\":\"db_allnode\"},{\"node_type\":\"0x3ff11\",\"node_addr\":\"[00:13:a2:00:40:ad:58:kk]!\",\"node_status\":\"0\",\"node_io\":\"F8\",\"node_command\":\"db_allnode\"},{\"node_type\":\"0x3ff11\",\"node_addr\":\"[00:13:a2:00:40:b2:16:5a]!\",\"node_status\":\"0\",\"node_io\":\"FE\",\"node_command\":\"db_allnode\"},{\"node_type\":\"0xa001a\",\"node_addr\":\"[00:13:a2:00:40:ad:57:e3]!\",\"node_status\":\"0\",\"node_io\":\"FA\",\"node_command\":\"db_allnode\"}, {\"node_type\":\"0xa001c\",\"node_addr\":\"[00:13:a2:00:40:ad:57:ca]!\",\"node_status\":\"0\",\"node_io\":\"FA\",\"node_command\":\"db_allnode\"}]}");
							#endregion
							////no websocketMaster.Send("{\"cmd_login\":[{\"flagForLogin\":\"pass\",\"lastConnectWebscoketUrl\":\"ws://echo.websocket.org\"}]})");

							await App.Database.Save_Login_Item (username.Text, password.Text, data.flagForLogin, data.lastConnectWebscoketUrl);
							#region FirstSendToGateway
							Db_allnode db = new Db_allnode ();
							db.node_command = "db_allnode";
							db.ID = 0;
							db.nodeStatusToString = "";
							db.node_addr = "";
							db.node_deviceType = "";
							db.node_io = "";
							db.node_status = "";
							db.node_type = "";
							db.node_io_p = "";

							var FirstSend = JsonConvert.SerializeObject(db);
							await ws_client.SendAsync (FirstSend);

							#endregion

						}else if (data.flagForLogin.Equals("not_pass")){

							Device.BeginInvokeOnMainThread (async () => {
								UserDialogs.Instance.ErrorToast("Waring","Username or Password is Invalid");
								//LoginPage.loginButton.IsEnabled = true;
								//LoginPage.logoutButton.IsEnabled = true;
								//LoginPage.activityIndicator.IsRunning = false;
								//LoginPage.loginFail.IsVisible = true;
								//MessageBarManager.SharedInstance.ShowMessage("Waring", "Username or Password is Invalid", MessageType.Error);
							});
							await ws_client.CloseAsync();

						}else if (data.flagForLogin.Equals("add_user_success")){
							await App.Database.Add_Login_Username_Show_For_Del(data.username);

							Device.BeginInvokeOnMainThread (async () => {

								//MessageBarManager.SharedInstance.ShowMessage("Account", data.username + " Added" , MessageType.Success);
							});
						}else if (data.flagForLogin.Equals("user_exits")){
							Device.BeginInvokeOnMainThread (async () => {

								//MessageBarManager.SharedInstance.ShowMessage("Account", data.username + " Already in use" , MessageType.Error);
							});

						}else if (data.flagForLogin.Equals("user_password_change")){
							foreach (var LoginData in await App.Database.Get_flag_Login()) //check wa koiy login? 
							{
								await App.Database.Delete_Login_Item ();
								//await App.Database.Save_Login_Item(LoginData.username, Change_Password_Page.newPassword.Text, LoginData.flagForLogin, LoginData.lastConnectWebscoketUrl);
								break;
							}
							Device.BeginInvokeOnMainThread (async () => {

								//MessageBarManager.SharedInstance.ShowMessage("Password", "\"New Password is Changed" , MessageType.Success);
							});
						}else if (data.flagForLogin.Equals("query_user")){

							await App.Database.Add_Login_Username_Show_For_Del(data.username);

						}else if (data.flagForLogin.Equals("user_deleted")){

							await App.Database.Delete_Login_Username_Show_For_Del(data.username);
							Device.BeginInvokeOnMainThread (async () => {
								//Admin_Delete_User_Page.usernameForDelete.ItemsSource = await App.Database.Get_Login_Username_Show_For_Del();
								//MessageBarManager.SharedInstance.ShowMessage("Account", data.username + " is Deleted" , MessageType.Success);
							});	
						}
					}

				}else if(cmd.cmd_remote != null){

					foreach(var data in cmd.cmd_remote)
					{
						//Log.Info ("cmd_remote" , "cmd_remote");

						foreach (var checkUser in await App.Database.Get_flag_Login())  
						{
							if(checkUser.username == data.remote_username){
								if(data.node_command.Equals("remote_code_success")){
									
									await App.Database.Save_RemoteData_Item(data.node_addr, data.remote_button_name, data.remote_username);
									MessagingCenter.Send<Add_Remote_Button> (new Add_Remote_Button(), "remote_code_success");

								}else if(data.node_command.Equals("remote_code_sync_database")){

									await App.Database.Save_RemoteData_Item(data.node_addr, data.remote_button_name, data.remote_username);

								}else if(data.node_command.Equals("remote_code_fail")){

									UserDialogs.Instance.ShowError("Try again..");

								}else if(data.node_command.Equals("name_exist")){

									UserDialogs.Instance.ShowError("Name_exist..");

								}else if(data.node_command.Equals("remote_send_success")){

									Device.BeginInvokeOnMainThread (async () => {
										//var notificator = DependencyService.Get<IToastNotificator>();
										//await notificator.Notify(ToastNotificationType.Success, 
										//	"Success", " Remote code success", TimeSpan.FromSeconds(1));
									});

								}else if(data.node_command.Equals("delete_remote_success")){
									
									await App.Database.Delete_RemoteData_Custom_Item(data.remote_button_name, data.remote_username);
									MessagingCenter.Send<RemotePage> (new RemotePage(), "delete_remote_success");

								}else if(data.node_command.Equals("rename_remote_success")){
									
									await App.Database.Rename_RemoteData_Item(data.remote_button_name, data.new_button_name, data.remote_username);
									MessagingCenter.Send<RemotePage> (new RemotePage(), "rename_remote_success");

								}
							}
							break;
						}


					}
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine ("websocket_MessageReceived Exception\n {0}", ex);
			}
		}


		private string find_io_value(int position, string node_io)
		{
			string state = NumberConversion.hex2binary (node_io);
			//System.Diagnostics.Debug.WriteLine("testtttttt + " + state);
			string io_state = string.Empty;
			if(position.ToString().Equals("0")){
				io_state = state.Substring(7, 1);
				//System.Diagnostics.Debug.WriteLine("io_state 7,1 " + io_state);
				if (io_state.Equals ("0")) 
				{ 
					//System.Diagnostics.Debug.WriteLine("ecccc1111");
					io_state = "true";
				} 
				else 
				{
					io_state = "false";
				}
			}else if(position.ToString().Equals("1")){
				io_state = state.Substring(6, 1);
				//System.Diagnostics.Debug.WriteLine("io_state 6,1 " + io_state);
				if (io_state.Equals ("0")) 
				{
					//System.Diagnostics.Debug.WriteLine("ecccc2222");
					io_state = "true";
				} 
				else 
				{
					io_state = "false";
				}
			}else if(position.ToString().Equals("2")){
				io_state = state.Substring(5, 1);
				//System.Diagnostics.Debug.WriteLine("io_state 5,1 " + io_state);
				if (io_state.Equals ("0")) 
				{ 
					io_state = "true";
				} 
				else 
				{
					io_state = "false";
				}
			}else if(position.ToString().Equals("3")){
				io_state = state.Substring(4, 1);
				//System.Diagnostics.Debug.WriteLine("io_state 4,1 " + io_state);
				if (io_state.Equals ("0")) 
				{ 
					io_state = "true";
				} 
				else 
				{
					io_state = "false";
				}
			}

			return io_state;
		}
	}
}

