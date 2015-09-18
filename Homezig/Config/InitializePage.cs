using System;
using Xamarin.Forms;

namespace Homezig
{
	public class InitializePage : ContentPage
	{
		public static Node_io_GpdPage ni_gpd;
		public static DeviceAddressListPage da_l;
		public static Node_io_ItemPage ni_iw;


		public InitializePage ()
		{
			ni_gpd = new Node_io_GpdPage ();
			da_l = new DeviceAddressListPage ();
			ni_iw = new Node_io_ItemPage ();
		}
	}
}

