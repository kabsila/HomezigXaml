using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class Node_io_Item_Edit : ContentPage
	{
		public Node_io_Item_Edit ()
		{
			InitializeComponent ();
		}

		public async void OnSave (object sender, EventArgs e) 
		{
			var todoItem = (NameByUser)BindingContext;	
			todoItem.io_name_by_user = nameEntry.Text;
			await App.Database.Update_NameByUser_by_target_io(nameEntry.Text, todoItem.node_addr, todoItem.node_io_p);
			await Navigation.PopAsync();
		}

		public async void OnCancel (object sender, EventArgs e) 
		{
			await Navigation.PopAsync();
		}
	}
}

