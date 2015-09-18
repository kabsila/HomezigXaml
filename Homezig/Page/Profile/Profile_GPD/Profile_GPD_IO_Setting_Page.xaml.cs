using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class Profile_GPD_IO_Setting_Page : ContentPage
	{
		public Profile_GPD_IO_Setting_Page ()
		{
			InitializeComponent ();
		}

		async void OnSwitchChange(object sender, ToggledEventArgs e)
		{
			var item = (ProfileData)BindingContext;
			var secureMode = Convert.ToString(e.Value);
			System.Diagnostics.Debug.WriteLine("{0}", secureMode);
			await App.Database.Update_Profile_IO_Data_SecurityMode(item.profileName, item.nodeAddrOfProfile, secureMode);
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			var item = (ProfileData)BindingContext;
			NameOfNode.Text = item.NameByUserNodeOfProfile;

			foreach(var data in await App.Database.Get_Profile_IO_Data_By_Addr(item.nodeAddrOfProfile, item.profileName))
			{
				alert_mode.IsToggled = Convert.ToBoolean(data.alert_mode);
				break;
			}
		}
	}
}

