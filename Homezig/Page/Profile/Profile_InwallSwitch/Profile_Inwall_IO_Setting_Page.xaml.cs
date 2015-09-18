using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class Profile_Inwall_IO_Setting_Page : ContentPage
	{
		public Profile_Inwall_IO_Setting_Page ()
		{
			InitializeComponent ();
		}

		public async void OnToggle (object sender, ToggledEventArgs e) 
		{
			var b = (Switch)sender;
			var IO_ProfileData = (Profile_IO_Data)b.BindingContext;
			var alert_mode = "False";
			//IO_ProfileData.io_value = Convert.ToByte (e.Value).ToString ();
			IO_ProfileData.io_value = e.Value.ToString();
			try
			{
				await App.Database.Update_Profile_IO_Data(IO_ProfileData.profileName, IO_ProfileData.node_addr, IO_ProfileData.node_io_p, IO_ProfileData.io_value, alert_mode);
				//await App.Database.Insert_Profile_IO_Data(Profile_Page.profileName, IO_ProfileData.node_addr, IO_ProfileData.node_io_p, IO_ProfileData.io_value, alert_mode);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine (ex);
			}
			
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			var item = (ProfileData)BindingContext;

			ProfileInwallIoListView.BindingContext = await App.Database.Get_Profile_IO_Data_By_Addr(item.nodeAddrOfProfile, item.profileName);
			//ProfileInwallIoListView.ItemsSource = await App.Database.Get_Profile_IO_Data_By_Addr(item.nodeAddrOfProfile, Profile_Page.profileName);
		}
	}
}

