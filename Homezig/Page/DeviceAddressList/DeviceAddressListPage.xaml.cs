using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class DeviceAddressListPage : ContentPage
	{
		public DeviceAddressListPage ()
		{
			InitializeComponent ();

			addressListView.ItemTapped  += (sender, e) => 
			{	

				var Item = (Db_allnode)e.Item;
				if (Item.node_deviceType.Equals (EnumtoString.EnumString(NodeDeviceType.GeneralPurposeDetector)) && Item.node_status.Equals("0")) {
					var DeviceList = new Node_io_GpdPage ();
					DeviceList.BindingContext = Item;
					Navigation.PushAsync (DeviceList);
				} else if (Item.node_deviceType.Equals (EnumtoString.EnumString(NodeDeviceType.InWallSwitch)) && Item.node_status.Equals("0")) {
					var DeviceList = new Node_io_ItemPage();//InitializePage.ni_iw;//new Node_io_ItemPage ();
					DeviceList.BindingContext = Item;
					Navigation.PushAsync (DeviceList);
				} else if (Item.node_deviceType.Equals (EnumtoString.EnumString(NodeDeviceType.RemoteControl)) && Item.node_status.Equals("0")) {
					var DeviceList = new RemotePage ();
					DeviceList.BindingContext = Item;
					Navigation.PushAsync (DeviceList);
				} 
				else {
					((ListView)sender).SelectedItem = null; //disable listview hightLight
				}
			};
		}

		public void OnEdit (object sender, EventArgs e) {
			var mi = ((MenuItem)sender);
			var mo = (Db_allnode)mi.BindingContext;
			var DeviceList = new DeviceAddressList_Edit ();
			DeviceList.BindingContext = mo;
			Navigation.PushAsync (DeviceList);
			System.Diagnostics.Debug.WriteLine("DeviceAddressList_EditActionClicked");
		}

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();

			var deviceType = (Db_allnode)BindingContext;
			var dataSource =  await App.Database.GetItemByDeviceType(deviceType.node_deviceType.ToString());

			foreach (var data in dataSource) 
			{
				if (data.name_by_user == null) {
					data.name_by_user = data.node_addr;
					await App.Database.Update_DBAllNode_Item(data);
				} 

				if (data.node_status.Equals ("1")) { // 1 is OFFLINE , 0 is ONLINE
					var indexForRemove = data.name_by_user.IndexOf ("(OffLine)");
					if (indexForRemove == -1) {
						data.name_by_user = data.name_by_user + "(OffLine)";
					} else {
						data.name_by_user = data.name_by_user.Remove (indexForRemove);
						data.name_by_user = data.name_by_user + "(OffLine)";
					}
					await App.Database.Update_DBAllNode_Item (data);

				} else {
					var indexForRemove = data.name_by_user.IndexOf ("(OffLine)");						
					if (indexForRemove != -1) {
						data.name_by_user = data.name_by_user.Remove (indexForRemove);
						await App.Database.Update_DBAllNode_Item (data);
					}
				}
			}
			addressListView.ItemsSource = dataSource;
		}
	}


}

