using System;
using System.Collections.Generic;
using Acr.DeviceInfo;
using Xamarin.Forms;
using System.Globalization;

namespace Homezig
{
	public partial class DeviceTypeListPage : ContentPage
	{
		public DeviceTypeListPage ()
		{
			InitializeComponent ();

			System.Diagnostics.Debug.WriteLine ("ScreenHeight = {0}",DeviceInfo.Hardware.ScreenHeight);
			System.Diagnostics.Debug.WriteLine ("ScreenWidth = {0}",DeviceInfo.Hardware.ScreenWidth);

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

	class ScreenConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{			
			double divider;
			Double.TryParse (parameter as string, out divider);
			if (DeviceInfo.Hardware.ScreenHeight <= 480) {
				divider = 7;
			} else {
				divider = 6;
			}
			return (double)DeviceInfo.Hardware.ScreenHeight / divider;

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{			
			double divider;
			Double.TryParse (parameter as string, out divider);
			return (double)DeviceInfo.Hardware.ScreenHeight / divider;
		}
	}

	class ImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var imageName = string.Empty;
			var input = value as string;
			if (input.Equals (EnumtoString.EnumString(NodeDeviceType.InWallSwitch))) {
				imageName = "DeviceType/bolt.png";
			}else if(input.Equals (EnumtoString.EnumString(NodeDeviceType.GeneralPurposeDetector))){
				imageName = "DeviceType/gate.png";
			}else if(input.Equals (EnumtoString.EnumString(NodeDeviceType.RemoteControl))){
				imageName = "DeviceType/remote.png";
			}else if(input.Equals (EnumtoString.EnumString(NodeDeviceType.Camera))){
				imageName = "DeviceType/webcam.png";
			}
			return imageName;

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var imageName = string.Empty;	
			if (value.Equals (NodeDeviceType.InWallSwitch)) {
				imageName = "arrow_right_grey.png";
			}
			return imageName;
		}
	}
}

