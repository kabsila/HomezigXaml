using System;
using SQLite.Net;
using SQLite.Net.Async;
using Xamarin.Forms;


namespace Homezig
{
	public class App : Application
	{
		static DeviceItemDatabase database;
		//public static App current;
		//public static INavigation Navigation 
		//{
		//	get;
		//	set;
		//}

		public App ()
		{
			// The root page of your application
			//new DeviceItemDatabase ();
			//MainPage = new NavigationPage(new LoginPage());
			//current = this;
			new InitializePage();

			MainPage = new LoginPage();


			//MainPage = new MenuTabPage ();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts

		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		public static DeviceItemDatabase Database 
		{
			get 
			{ 
				if (database == null) {
					database = new DeviceItemDatabase ();
				}
				return database; 
			}
		}
	}
}

