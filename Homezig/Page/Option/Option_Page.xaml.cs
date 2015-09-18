using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class Option_Page : ContentPage
	{
		
		ImageCell adduser = new ImageCell 
		{
			Text = "Add new user",
			ImageSource = "OptionPage/AddUser-25.png"
		};

		ImageCell deluser = new ImageCell 
		{
			Text = "Delete user",
			ImageSource = "OptionPage/RemoveUser-25.png"
		};

		ImageCell changePassword = new ImageCell 
		{
			Text = "Change password",
			ImageSource = "OptionPage/Password-25.png"
		};

		ImageCell logout = new ImageCell 
		{
			Text = "Log Out",
			ImageSource = "OptionPage/Exit-25.png"
		};

		public Option_Page ()
		{
			InitializeComponent ();

			Command<Type> navigateCommand = new Command<Type>(async (Type pageType) =>
				{
					Page page = (Page)Activator.CreateInstance(pageType);
					await this.Navigation.PushAsync(page);
				});

			Command<Type> logoutCommand = new Command<Type>(async (Type pageType) =>
				{
					await App.Database.Delete_Login_Item();
					await App.Database.Delete_RemoteData_Item();
					await App.Database.Delete_All_Login_Username_Show_For_Del ();

					//await LoginPage.ws_client.SendAsync("testrrrrr");
					//await System.Threading.Tasks.Task.Delay(3000);
					await LoginPage.ws_client.CloseAsync();

					await Navigation.PopModalAsync();			

				});

			adduser.Command = navigateCommand;
			adduser.CommandParameter = typeof(Admin_Add_User_Page);

			deluser.Command = navigateCommand;
			deluser.CommandParameter = typeof(Admin_Delete_User_Page);

			changePassword.Command = navigateCommand;
			changePassword.CommandParameter = typeof(Change_Password_Page);

			logout.Command = logoutCommand;
			logout.CommandParameter = typeof(LoginPage);

			Check_admin_for_add_menu ();
		}

		async void Check_admin_for_add_menu()
		{
			foreach (var data in await App.Database.Get_flag_Login()) 
			{
				if (data.username.Equals ("admin")) {
					ts.Add (adduser);
					ts.Add (deluser);
					ts.Add (logout);
					//ts.Add (changePassword); // add for test
				} else {
					ts.Add (changePassword);
					ts.Add (logout);
				}
				break;
			}

		}
	}
}

