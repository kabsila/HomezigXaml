using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Connectivity.Plugin.Abstractions;
using Connectivity.Plugin;
using Acr.UserDialogs;

using Syncfusion.SfChart.XForms.iOS.Renderers;

namespace Homezig.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			new SfChartRenderer();
			// Code for starting up the Xamarin Test Cloud Agent
			//#if ENABLE_TEST_CLOUD
			//Xamarin.Calabash.Start();
			//#endif

			LoadApplication (new App ());

			//UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

			return base.FinishedLaunching (app, options);
		}


	}
}

