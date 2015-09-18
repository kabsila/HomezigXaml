using System;
using Homezig;
using Xamarin.Forms;
using Homezig.Droid;
using System.IO;
using SQLite.Net;
using SQLite.Net.Async;
using System.Threading.Tasks;

[assembly: Dependency (typeof (SQLite_Android))]

namespace Homezig.Droid
{
	public class SQLite_Android : ISQLite
	{
		public SQLite_Android ()
		{
		}

		#region ISQLite implementation
		public SQLiteAsyncConnection GetConnection ()
		{
			var sqliteFilename = "HomezigSQLite.db3";
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal); // Documents folder
			var path = Path.Combine(documentsPath, sqliteFilename);

			// This is where we copy in the prepopulated database
			//Console.WriteLine (path);
			if (!File.Exists(path))
			{
				//var s = Forms.Context.Resources.OpenRawResource(Resource.Raw.TodoSQLite);  // RESOURCE NAME ###

				// create a write stream
				//FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
				new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
				// write to the stream
				//ReadWriteStream(s, writeStream);
			}

			var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			//var conn = new SQLite.Net.Async.SQLiteAsyncConnection(plat, path);

			var connectionFactory = new Func<SQLiteConnectionWithLock>(()=>new SQLiteConnectionWithLock(plat, new SQLiteConnectionString(path, storeDateTimeAsTicks: false)));
			var conn = new SQLiteAsyncConnection(connectionFactory);
			//conn.ExecuteAsync ("PRAGMA encoding = UTF8");
			// Return the database connection 
			return conn;
		}
		#endregion

		/// <summary>
		/// helper method to get the database out of /raw/ and into the user filesystem
		/// </summary>
		void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			int Length = 256;
			Byte[] buffer = new Byte[Length];
			int bytesRead = readStream.Read(buffer, 0, Length);
			// write the required bytes
			while (bytesRead > 0)
			{
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, Length);
			}
			readStream.Close();
			writeStream.Close();
		}
	}
}