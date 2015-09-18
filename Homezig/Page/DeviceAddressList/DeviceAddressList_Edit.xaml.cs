using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class DeviceAddressList_Edit : ContentPage
	{
		public DeviceAddressList_Edit ()
		{
			InitializeComponent ();
		}

		public async void OnSave (object sender, EventArgs e) {
			var todoItem = (Db_allnode)BindingContext;	

			todoItem.name_by_user = nameEntry.Text;
			await App.Database.Update_Node_NameByUser(todoItem.name_by_user, todoItem.node_addr);
			await App.Database.Update_NameByUser(todoItem.name_by_user, todoItem.node_addr);
			await Navigation.PopAsync();
		}

		public async void OnCancel (object sender, EventArgs e) {
			var todoItem = (Db_allnode)BindingContext;
			await Navigation.PopAsync();
		}
	}
}

