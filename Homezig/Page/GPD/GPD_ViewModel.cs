using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Homezig
{
	public class GPD_ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		ObservableCollection<NameByUser> _listviewItem;

		public ObservableCollection<NameByUser> ListviewItem
		{
			set
			{
				if (!value.Equals(_listviewItem))
				{
					_listviewItem = value;
					OnPropertyChanged("ListviewItem");

				}
			}
			get
			{
				return _listviewItem;
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

