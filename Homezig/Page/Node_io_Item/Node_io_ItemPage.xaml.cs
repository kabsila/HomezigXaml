using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite.Net;
using SQLite.Net.Async;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Homezig
{
	public partial class Node_io_ItemPage : ContentPage
	{
		
		//List<NameByUser> employees = new List<NameByUser>();

		public static  ObservableCollection<NameByUser> obsCol { get; set;}
		bool checkLoop = false;
		node_io_viewModel Node_io_Item_ViewModel;

		public Node_io_ItemPage ()
		{
			InitializeComponent ();

			Node_io_Item_ViewModel = new node_io_viewModel ();

			MessagingCenter.Subscribe<Node_io_ItemPage, string> (this, "Node_io_ItemPage_Io_Change", async (sender, arg) => {				
				//var item = (Db_allnode)BindingContext;
				checkLoop = false;
				//ioListView.ItemsSource = new ObservableCollection<NameByUser> (await App.Database.Get_NameByUser_by_addr(arg));
				//ioListView.BindingContext = new ObservableCollection<NameByUser> (await App.Database.Get_NameByUser_by_addr(arg));
				Node_io_Item_ViewModel.ListviewItem = new ObservableCollection<NameByUser> (await App.Database.Get_NameByUser_by_addr(arg));
			});

			ioListView.PropertyChanged += (sender, e) => 
			{				
				checkLoop = false;
				System.Diagnostics.Debug.WriteLine("PropertyChanged");
			};

		}



		public async void onclick (object sender, EventArgs e)
		{			
			await LoginPage.ws_client.SendAsync ("{\"cmd_db_allnode\":[{\"node_type\":\"0x3ff01\",\"node_addr\":\"[00:13:a2:00:40:ad:58:ab]!\",\"node_status\":\"0\",\"node_io\":\"FD\",\"node_command\":\"io_change_detected\"}]}");

		}

		public void OnEdit (object sender, EventArgs e) 
		{
			var mi = ((MenuItem)sender);
			var mo = (NameByUser)mi.BindingContext;
			var DeviceList = new Node_io_Item_Edit ();
			DeviceList.BindingContext = mo;
			Navigation.PushAsync (DeviceList);
		}


		async void onToggled(object sender, ToggledEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine ("send  {0}", "Success");
			if (checkLoop) {
				try
				{
					var b = (Switch)sender;
					var NameByUserData = (NameByUser)b.BindingContext;			
				
					string io_state = string.Empty;
					var aStringBuilder = new StringBuilder (NumberConversion.hex2binary (NameByUserData.node_io));
					if (NameByUserData.node_io_p.Equals ("0")) {
						io_state = Convert.ToByte (e.Value).ToString ();
						aStringBuilder.Remove (7, 1);
						aStringBuilder.Insert (7, io_state);

					} else if (NameByUserData.node_io_p.Equals ("1")) {
						io_state = Convert.ToByte (e.Value).ToString ();
						aStringBuilder.Remove (6, 1);
						aStringBuilder.Insert (6, io_state);

					}

					//Node_io_ItemPage.NBitem.node_io = NumberConversion.binary2hex (aStringBuilder.ToString ());
					NameByUserData.node_io = NumberConversion.binary2hex (aStringBuilder.ToString ());//Node_io_ItemPage.NBitem.node_io;
					NameByUserData.node_command = "command_io";
					string jsonCommandIo = JsonConvert.SerializeObject (NameByUserData, Formatting.Indented);
					System.Diagnostics.Debug.WriteLine ("send \n {0}", jsonCommandIo);


					//await App.Database.Update_NameByUser_ioValue(NameByUserData.node_io, NameByUserData.node_addr);
					await App.Database.Update_NameByUser_ioValue2 (NameByUserData.node_io, io_state, NameByUserData.node_addr, NameByUserData.node_io_p);
					await LoginPage.ws_client.SendAsync (jsonCommandIo);
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine ("onToggled Exceptionn \n {0}", ex);
				}

			} 
			checkLoop = true;


		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			var item = (Db_allnode)BindingContext;

			//var vm = new Node_io_viewModel ();
			//vm.List = new ObservableCollection<NameByUser>( await App.Database.Get_NameByUser_by_addr (item.node_addr));
			//obsCol = new ObservableCollection<NameByUser> (await App.Database.Get_NameByUser_by_addr(item.node_addr));
			//ioListView.ItemsSource = obsCol;//await App.Database.Get_NameByUser_by_addr(item.node_addr);
			//ioListView.BindingContext = obsCol;


			Node_io_Item_ViewModel.ListviewItem = new ObservableCollection<NameByUser>( await App.Database.Get_NameByUser_by_addr(item.node_addr));
			ioListView.BindingContext = Node_io_Item_ViewModel;


		}
	}
}

