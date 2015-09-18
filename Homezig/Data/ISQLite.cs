using System;
using SQLite.Net;
using SQLite.Net.Async;

namespace Homezig
{
	public interface ISQLite
	{
		SQLiteAsyncConnection GetConnection();
	}
}

