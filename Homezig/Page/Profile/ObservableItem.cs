using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Homezig
{
	/**public class Item
	{
		public string profile_status { get; set; }
		public string profileName { get; set; }
	}**/

	public class ObservableItem : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		ProfileData item;

		public ObservableItem()
		{
			item = new ProfileData();
		}

		public string profile_status
		{
			set
			{
				if (!value.Equals(item.profile_status, StringComparison.Ordinal))
				{
					item.profile_status = value;
					OnPropertyChanged("profile_status");
				}
			}
			get
			{
				return item.profile_status;
			}
		}

		public string profileName
		{
			set
			{
				if (!value.Equals(item.profileName, StringComparison.Ordinal))
				{
					item.profileName = value;
					OnPropertyChanged("profileName");
				}
			}
			get
			{
				return item.profileName;
			}
		}

		public string NameByUserNodeOfProfile
		{
			set
			{
				if (!value.Equals(item.NameByUserNodeOfProfile, StringComparison.Ordinal))
				{
					item.NameByUserNodeOfProfile = value;
					OnPropertyChanged("NameByUserNodeOfProfile");
				}
			}
			get
			{
				return item.NameByUserNodeOfProfile;
			}
		}

		public string nodeAddrOfProfile
		{
			set
			{
				if (!value.Equals(item.nodeAddrOfProfile, StringComparison.Ordinal))
				{
					item.nodeAddrOfProfile = value;
					OnPropertyChanged("nodeAddrOfProfile");
				}
			}
			get
			{
				return item.nodeAddrOfProfile;
			}
		}

		public string node_deviceType
		{
			set
			{
				if (!value.Equals(item.node_deviceType, StringComparison.Ordinal))
				{
					item.node_deviceType = value;
					OnPropertyChanged("node_deviceType");
				}
			}
			get
			{
				return item.node_deviceType;
			}
		}

		public string node_command
		{
			set
			{
				if (!value.Equals(item.node_command, StringComparison.Ordinal))
				{
					item.node_command = value;
					OnPropertyChanged("node_command");
				}
			}
			get
			{
				return item.node_command;
			}
		}

		public int ID
		{
			set
			{
				if (!value.Equals(item.ID))
				{
					item.ID = value;
					OnPropertyChanged("ID");
				}
			}
			get
			{
				return item.ID;
			}
		}

		void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
