using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace Homezig
{
	public class AlertNotification
	{
		public AlertNotification ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string node_addr { get; set; }
		public string node_type { get; set; }
		public string node_status { get; set; }
		public string node_io { get; set; }
		public string node_command { get; set; }
		//public string node_name_by_user { get; set; }
		//public string io_name_by_user { get; set; }
		//public string node_deviceType { get; set; }


	}
}

