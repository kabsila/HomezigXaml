using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class DeviceTypeListPage : ContentPage
	{
		public DeviceTypeListPage ()
		{
			InitializeComponent ();

			typeListView.ItemSelected += (sender, e) => {
				var Item = (Db_allnode)e.SelectedItem;
				var DeviceList = new DeviceAddressListPage();
				DeviceList.BindingContext = Item;
				Navigation.PushAsync(DeviceList);
				//new NavigationPage().Navigation.PushAsync(DeviceList);
				//App.Navigation.Navigation.PopModalAsync();


			};

		}	

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();
			typeListView.ItemsSource = await App.Database.GetItemGroupByDeviceType ();

		}
	}
}

