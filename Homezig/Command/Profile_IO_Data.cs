using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace Homezig
{
	public class Profile_IO_Data
	{
		public Profile_IO_Data ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string profileName { get; set; }
		public string io_name_by_user { get; set; }
		public string node_addr { get; set; }
		public string node_deviceType { get; set; }
		public string node_io_p { get; set; }
		public string io_value { get; set; }
		public string alert_mode { get; set; }
		public string node_command { get; set; }
	}
}

