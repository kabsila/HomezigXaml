using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Homezig
{
	public partial class MenuTabPage : TabbedPage
	{
		public MenuTabPage ()
		{
			InitializeComponent ();


			this.Children.Add (new NavigationPage(new DeviceTypeListPage()){Title="Powered", Icon="MenuTapPage/Contact.png"});
			this.Children.Add (new NavigationPage(new Profile_Page()){Title = "Profile", Icon="MenuTapPage/Contact.png"});
			this.Children.Add (new NavigationPage(new Option_Page()){Title = "Option", Icon="MenuTapPage/Contact.png"});
			//this.Children.Add (new NavigationPage(new Option_Page(ipm)){Title = "Option"});
		}
	}
}

