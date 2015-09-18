using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Homezig
{
	public partial class Add_Profile_Page : ContentPage
	{
		public List<ProfileData> ProfileDataList = new List<ProfileData> ();
		public Add_Profile_Page ()
		{
			InitializeComponent ();
		}

		public async void OnCreateProfile (object sender, EventArgs e) {
			if (String.IsNullOrEmpty(nameEntry.Text) || !ProfileDataList.Any()) //check list is not null
			{
				await DisplayAlert("Validation Error", "Profile name are required or Node not selected", "Re-try");
			} 
			else 
			{
				foreach(var item in ProfileDataList)
				{
					item.profileName = nameEntry.Text;
					item.profile_status = "False";
				}

				Profile_Page.refreshListview (nameEntry.Text, "False");
				//await App.Database.Insert_ProfileData_Item(nameEntry.Text);
				await App.Database.Insert_ProfileData_Item(ProfileDataList);
				await Navigation.PopAsync();
				ProfileDataList.Clear();
			}
		}

		public async void OnCancel (object sender, EventArgs e) {
			ProfileDataList.Clear();
			var todoItem = (ProfileData)BindingContext;
			await Navigation.PopAsync();
		}

		public void switch_Toggled(object sender, ToggledEventArgs e)
		{
			var b = (Switch)sender;
			var ItemData = (NameByUser)b.BindingContext;

			ProfileData ProfileData = new ProfileData ();

			if (e.Value) {				
				ProfileData.NameByUserNodeOfProfile = ItemData.node_name_by_user;
				ProfileData.nodeAddrOfProfile = ItemData.node_addr;
				ProfileData.node_deviceType = ItemData.node_deviceType;
				ProfileDataList.Add (ProfileData);
			} else {
				ProfileDataList.RemoveAll (x => x.nodeAddrOfProfile == ItemData.node_addr);
			}

		}

		protected override async void OnAppearing()
		{
			base.OnAppearing ();
			deviceListview.ItemsSource = await App.Database.Get_NameByUser_GroupBy_Addr();

		}
	}
}

