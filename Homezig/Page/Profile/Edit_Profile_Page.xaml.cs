using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class Edit_Profile_Page : ContentPage
	{
		string oldName = string.Empty;


		public Edit_Profile_Page ()
		{
			InitializeComponent ();
		}

		public async void OnSave (object sender, EventArgs e) 
		{	
			await App.Database.Edit_ProfileData_Item(nameEntry.Text, oldName);
			await Navigation.PopAsync();
		}

		public async void OnCancel (object sender, EventArgs e) 
		{
			await Navigation.PopAsync();
		}

		protected override void OnAppearing ()
		{
			oldName = nameEntry.Text;
		}

	}
}

