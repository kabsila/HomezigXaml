using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace Homezig
{
	public class LoginUsernameForDel
	{
		public LoginUsernameForDel ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string username { get; set; }
		public string node_command { get; set; }

	}
}

