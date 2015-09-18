using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace Homezig
{
	public class RemoteData
	{
		public RemoteData ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string remote_username { get; set; }
		public string node_addr { get; set; }
		public string node_command { get; set; }
		public string remote_button_name { get; set; }
		public string remote_code { get; set; }
		public string new_button_name { get; set; }

	}
}

