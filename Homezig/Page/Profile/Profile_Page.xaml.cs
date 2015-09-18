using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using Newtonsoft.Json;
using WebSocket.Portable.Net;
using WebSocket.Portable;
using System.Threading.Tasks;

namespace Homezig
{
	
	public partial class Profile_Page : ContentPage
	{
		static ObservableCollection<ProfileData> ListviewItem;


		bool checkLoop;

		int c = 0;

		public Profile_ViewModel _Profile_ViewModel;

		public Profile_Page ()
		{
			InitializeComponent ();

			_Profile_ViewModel = new Profile_ViewModel();
			c = 1;
			getDataToListview ();		

		}

		public async Task getDataToListview()
		{			
			ListviewItem = new ObservableCollection<ProfileData> (await App.Database.Get_ProfileName_GroupBy_Addr());
			ProfileListview.BindingContext = ListviewItem;
		}

		public static void refreshListview(string profileName, string profile_status)
		{
			using (ProfileData pd = new ProfileData ()) 
			{
				pd.profileName = profileName;
				pd.profile_status = profile_status;

				Device.BeginInvokeOnMainThread (()=>
				{
					ListviewItem.Add(pd);
				});
			}
			
		}
		public async void onclick (object sender, EventArgs e)
		{		
			await this.Navigation.PushAsync(new Add_Profile_Page());
		}

		public void OnEdit (object sender, EventArgs e) {
			var mi = ((MenuItem)sender);
			var mo = (ProfileData)mi.BindingContext;

			var profileList = new Edit_Profile_Page ();
			profileList.BindingContext = mo;
			//profileName =  mo.profileName;
			Navigation.PushAsync (profileList);

		}

		public void pc (object sender, PropertyChangingEventArgs e) {


			if (e.PropertyName == "IsToggled") {
				


				if (c == 1) {
					
					checkLoop = false;
					c = 0;

				} else {
					checkLoop = true;

				}



				System.Diagnostics.Debug.WriteLine ("is PropertyChangedEventArgs");
				//bcd = true;

			}
		}

		public void bc (object sender, EventArgs e) {
			
		
			System.Diagnostics.Debug.WriteLine ("is bc");
		}

		public async void OnDelete (object sender, EventArgs e) {
			var mi = ((MenuItem)sender);
			var mo = (ProfileData)mi.BindingContext;
			await App.Database.Delete_IO_Profile_By_ProfileName(mo.profileName);
			await App.Database.Delete_Profile_By_ProfileName(mo.profileName);

			foreach (var itemToRemove in ListviewItem.Where(x => x.profileName == mo.profileName).ToList())
			{				
				Device.BeginInvokeOnMainThread (()=>
				{
					ListviewItem.Remove(itemToRemove);
				});
			}

			//no ProfileListview.ItemsSource = await App.Database.Get_ProfileName_GroupBy_Addr();
		}

		public async void onItemTapped (object sender, ItemTappedEventArgs e) {
			var itemData = (ProfileData)e.Item;
			var DeviceList = new Profile_Setting_Page ();
			DeviceList.BindingContext = itemData;
			await Navigation.PushAsync (DeviceList);
		}

		public async void OnProfile_Toggled(object sender, ToggledEventArgs e)
		{
			
			//System.Diagnostics.Debug.WriteLine ("is in Toggled");

			if(checkLoop){
				
				//System.Diagnostics.Debug.WriteLine ("is in if");
				try
				{

					if (e.Value) {

						var b = (Switch)sender;

						var ProfileData = (ProfileData)b.BindingContext;
						var data = await App.Database.Get_Profile_IO_Data_By_ProfileName (ProfileData.profileName);
						if (!data.Any ()) 
						{
							c = 1;
							//check list is not null
							await DisplayAlert ("Validation Error", "Profile not setting", "OK");

							//await App.Database.Set_profile_Status (ProfileData.profileName, (!e.Value).ToString (), (!e.Value).ToString ());
							Device.BeginInvokeOnMainThread( async () => 
								{
									//_Profile_ViewModel.Items = await Profile_ViewModel.CreateAsync ();
									ProfileListview.BindingContext = await App.Database.Get_ProfileName_GroupBy_Addr();
								});
						} 
						else 
						{
							
							foreach (var item in data) 
							{							
								item.node_command = "Profile_Open";
								string jsonProfile = JsonConvert.SerializeObject (item, Formatting.Indented);
								await LoginPage.ws_client.SendAsync (jsonProfile);
								System.Diagnostics.Debug.WriteLine (jsonProfile);
							}
							//checkLoop = false;
							c = 1;

							await App.Database.Set_profile_Status (ProfileData.profileName, e.Value.ToString (), (!e.Value).ToString ());
							Device.BeginInvokeOnMainThread( async () => 
							{
								//_Profile_ViewModel.Items = await Profile_ViewModel.CreateAsync ();
									ProfileListview.BindingContext = await App.Database.Get_ProfileName_GroupBy_Addr();
							});
							
							//ProfileListview.BindingContext = await App.Database.Get_ProfileName_GroupBy_Addr ();
							//await System.Threading.Tasks.Task.Delay(5000);

						}

					}
					else 
					{
						c = 1;
						//System.Diagnostics.Debug.WriteLine ("is else");
						var b = (Switch)sender;
						var ProfileData = (ProfileData)b.BindingContext;
						System.Diagnostics.Debug.WriteLine (ProfileData.profileName);
						await App.Database.Set_profile_Status_Off (ProfileData.profileName, e.Value.ToString ());


						


						//server must close another profile
					}
				}
				catch(Exception ex)
				{					
					System.Diagnostics.Debug.WriteLine ("OnProfile_Toggled Exception \n {0}", ex);
				}
			}


		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();

			System.Diagnostics.Debug.WriteLine ("OnAppearing");

			await App.Database.Update_NameByUser_Profile ();
		}



	}


}

