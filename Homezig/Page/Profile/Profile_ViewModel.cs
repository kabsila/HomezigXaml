using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
namespace Homezig
{
	public class Profile_ViewModel
	{		
		ObservableCollection<ObservableItem> items;

		public Profile_ViewModel()
		{
			
			/**_listviewItem = new ObservableCollection<ObservableItem> { 
				new ObservableItem {profileName = "First", profile_status="True"}, 
				new ObservableItem {profileName = "Second", profile_status="False"},
				new ObservableItem {profileName = "Third", profile_status="True"} 
			};**/
		}

		public ObservableCollection<ObservableItem> Items
		{
			set
			{
				if ( value != items)
				{
					items = value;
				}
			}
			get
			{
				return items;
			}
		}


		public async Task<ObservableCollection<ObservableItem>> InitializeAsync()
		{
			items = new ObservableCollection<ObservableItem>();

			foreach(var data in await App.Database.Get_ProfileName_GroupBy_Addr())
			{
				ObservableItem ob = new ObservableItem ();
				ob.ID = data.ID;
				//ob.NameByUserNodeOfProfile = data.NameByUserNodeOfProfile;
				//ob.nodeAddrOfProfile = data.nodeAddrOfProfile;
				//ob.node_command = data.node_command;
				//ob.node_deviceType = data.node_deviceType;
				ob.profileName = data.profileName;
				ob.profile_status = data.profile_status;

				items.Add (ob);
			}
			System.Diagnostics.Debug.WriteLine ("InitializeAsync");
			return items;
		}

		public static Task<ObservableCollection<ObservableItem>> CreateAsync()
		{
			System.Diagnostics.Debug.WriteLine ("CreateAsync");
			var ret = new Profile_ViewModel();
			return ret.InitializeAsync();
		}


	}
}


