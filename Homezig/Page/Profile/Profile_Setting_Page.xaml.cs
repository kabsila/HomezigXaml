using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class Profile_Setting_Page : ContentPage
	{
		public Profile_Setting_Page ()
		{
			InitializeComponent ();
		}

		public async void onItemTapped (object sender, ItemTappedEventArgs e) 
		{
			var itemData = (ProfileData)e.Item;

			if(itemData.node_deviceType.Equals(EnumtoString.EnumString(NodeDeviceType.InWallSwitch))){
				var DeviceList = new Profile_Inwall_IO_Setting_Page ();
				DeviceList.BindingContext = itemData;
				await Navigation.PushAsync (DeviceList);
			}else if(itemData.node_deviceType.Equals(EnumtoString.EnumString(NodeDeviceType.GeneralPurposeDetector))){
				var DeviceList = new Profile_GPD_IO_Setting_Page ();
				DeviceList.BindingContext = itemData;
				await Navigation.PushAsync (DeviceList);
			}
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing ();
			var item = (ProfileData)BindingContext;
			//Profile_Page.profileName = item.profileName;
			//listOfNodeProfile.ItemsSource = await App.Database.Get_Node_For_Profile(item.profileName);
			listOfNodeProfile.BindingContext = await App.Database.Get_Node_For_Profile(item.profileName);
			var addr = await App.Database.Get_Addr_Of_ProfileName (item.profileName);
			var alert_mode = "False";
			foreach (var dataAddr in addr)
			{
				var tempData = await App.Database.Get_NameByUser_by_addr(dataAddr.nodeAddrOfProfile);
				foreach (var data in tempData)
				{
					await App.Database.Update_IO_Name (data.io_name_by_user, data.node_io_p, data.node_addr);
					await App.Database.Insert_Profile_IO_Data (item.profileName, data.node_addr, data.node_io_p, data.io_value, alert_mode, data.io_name_by_user, data.node_deviceType);
				}
			}

		}
	}
}

