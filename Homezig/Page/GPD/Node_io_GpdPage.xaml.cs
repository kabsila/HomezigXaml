using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Homezig
{
	public partial class Node_io_GpdPage : ContentPage
	{
		GPD_ViewModel gpdViewModel;
		public Node_io_GpdPage ()
		{
			InitializeComponent ();
			gpdViewModel = new GPD_ViewModel ();

			MessagingCenter.Subscribe<Node_io_GpdPage, string> (this, "Node_io_GpdPage_Io_Change", async (sender, arg) => {
				
				gpdViewModel.ListviewItem = new ObservableCollection<NameByUser> (await App.Database.Get_NameByUser_by_addr(arg));
			
			});

		}

		public void OnEdit (object sender, EventArgs e) {
			var mi = ((MenuItem)sender);
			var mo = (NameByUser)mi.BindingContext;

			var DeviceList = new Node_io_Gpd_Edit ();
			DeviceList.BindingContext = mo;
			Navigation.PushAsync (DeviceList);
		}

		public async void onclick (object sender, EventArgs e)
		{			
			await LoginPage.ws_client.SendAsync ("{\"cmd_db_allnode\":[{\"node_type\":\"0x3ff11\",\"node_addr\":\"[00:13:a2:00:40:ad:58:kk]!\",\"node_status\":\"0\",\"node_io\":\"FD\",\"node_command\":\"io_change_detected\"}]}");

		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			var item = (Db_allnode)BindingContext;
			gpdViewModel.ListviewItem = new ObservableCollection<NameByUser>( await App.Database.Get_NameByUser_by_addr(item.node_addr));
			ioListView.BindingContext = gpdViewModel;

			/**if (Config.addr.Equals (string.Empty))
			{
				var eitem = (Db_allnode)BindingContext;
				//NameOfNode.Text = item.name_by_user;

				ioListView.ItemsSource = await App.Database.Get_NameByUser_by_addr (item.node_addr);
			} else 
			{			
				NameOfNode.Text = Config.nameAddr;	
				ioListView.ItemsSource = await App.Database.Get_NameByUser_by_addr (Config.addr);
				Config.addr = string.Empty;
				Config.nameAddr = string.Empty;
			}**/

		}
	}
}

