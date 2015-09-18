using System;
using SQLite.Net;
using SQLite.Net.Async;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Homezig
{
	public class DeviceItemDatabase 
	{
		//static object locker = new object ();

		SQLiteAsyncConnection database;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public DeviceItemDatabase()
		{
			database = DependencyService.Get<ISQLite> ().GetConnection ();
			// create the tables

			database.CreateTableAsync<Login>();
			database.CreateTableAsync<LoginUsernameForDel>();
			database.CreateTableAsync<Db_allnode>();
			database.CreateTableAsync<NameByUser>();
			database.CreateTableAsync<RemoteData>();
			database.CreateTableAsync<ProfileData>();
			database.CreateTableAsync<Profile_IO_Data>();

			//prevent [node_addr] AND [node_io_p] duplicate
			database.QueryAsync<NameByUser>("CREATE UNIQUE INDEX ix_uq ON [NameByUser] ([node_addr], [node_io_p])");
			database.QueryAsync<Profile_IO_Data>("CREATE UNIQUE INDEX ix_uq2 ON [Profile_IO_Data] ([profileName], [node_addr], [node_io_p])");
		
		}
		#region Profile_IO_Data
		public async Task<IEnumerable<Profile_IO_Data>> Delete_IO_Profile_By_ProfileName (string profileName) 
		{
			return await database.QueryAsync<Profile_IO_Data>("DELETE FROM [Profile_IO_Data] WHERE [profileName] = ?", profileName);
		}

		public async Task<IEnumerable<Profile_IO_Data>> Insert_Profile_IO_Data (string profileName, string node_addr, string node_io_p, string io_value, string alert_mode, string io_name_by_user, string node_deviceType) 
		{
			return await database.QueryAsync<Profile_IO_Data>("INSERT INTO [Profile_IO_Data] ([profileName], [node_addr], [node_io_p], [io_value], [alert_mode], [io_name_by_user], [node_deviceType]) VALUES (?, ?, ?, ?, ?, ?, ?)",profileName, node_addr, node_io_p, io_value, alert_mode, io_name_by_user, node_deviceType);
		}

		public async Task<IEnumerable<Profile_IO_Data>> Update_IO_Name (string io_name_by_user, string node_io_p, string node_addr) 
		{
			return await database.QueryAsync<Profile_IO_Data>("UPDATE [Profile_IO_Data] SET [io_name_by_user] = ? WHERE [node_addr] = ? AND [node_io_p] = ?", io_name_by_user, node_addr, node_io_p);
		}

		public async Task<IEnumerable<Profile_IO_Data>> Update_Profile_IO_Data (string profileName, string node_addr, string node_io_p, string io_value, string alert_mode) 
		{
			return await database.QueryAsync<Profile_IO_Data>("UPDATE [Profile_IO_Data] SET [io_value] = ? , [alert_mode] = ? WHERE [node_addr] = ? AND [node_io_p] = ? AND [profileName] = ?",io_value, alert_mode, node_addr, node_io_p, profileName);
		}

		public async Task<IEnumerable<Profile_IO_Data>> Get_Profile_IO_Data_By_Addr (string node_addr, string profileName) 
		{
			return await database.QueryAsync<Profile_IO_Data>("SELECT * FROM [Profile_IO_Data] WHERE [node_addr] = ? AND [profileName] = ?",node_addr, profileName);
		}

		public async Task<IEnumerable<Profile_IO_Data>> Get_Profile_IO_Data () 
		{
			return await database.QueryAsync<Profile_IO_Data>("SELECT * FROM [Profile_IO_Data]");
		}

		public async Task<IEnumerable<Profile_IO_Data>> Get_Profile_IO_Data_By_Addr2 (string node_addr) 
		{
			return await database.QueryAsync<Profile_IO_Data>("SELECT * FROM [Profile_IO_Data] WHERE [node_addr] = ?",node_addr);
		}

		public async Task<IEnumerable<Profile_IO_Data>> Update_Profile_IO_Data_SecurityMode (string profileName, string node_addr, string alert_mode) 
		{
			return await database.QueryAsync<Profile_IO_Data>("UPDATE [Profile_IO_Data] SET [alert_mode] = ? WHERE [node_addr] = ? AND [profileName] = ?",alert_mode, node_addr, profileName);
		}

		public async Task<IEnumerable<Profile_IO_Data>> Get_Profile_IO_Data_By_ProfileName (string profileName) 
		{
			return await database.QueryAsync<Profile_IO_Data>("SELECT * FROM [Profile_IO_Data] WHERE [profileName] = ?", profileName);
		}



		/**public async Task<IEnumerable<Profile_IO_Data>> Get_Profile_IO_Data_By_Addr_And_Type (string node_addr, string profileName, string deviceType) 
		{
			return await database.QueryAsync<Profile_IO_Data>("SELECT * FROM [Profile_IO_Data] WHERE [node_addr] = ? AND [profileName] = ? AND [node_deviceType] = ?",node_addr, profileName, deviceType);
		}**/

		#endregion 

		#region ProfileData
		public async Task<int> Insert_ProfileData_Item (List<ProfileData> profileData)
		{
			return await database.InsertAllAsync (profileData);
		}

		public async Task<IEnumerable<ProfileData>> Get_Addr_Of_ProfileName (string profileName) 
		{
			return await database.QueryAsync<ProfileData>("SELECT DISTINCT [nodeAddrOfProfile] FROM [ProfileData] WHERE [profileName] = ?", profileName);
		}

		public async Task<IEnumerable<ProfileData>> Get_ProfileName_GroupBy_Addr () 
		{
			return await database.QueryAsync<ProfileData>("SELECT DISTINCT [profileName], [profile_status] FROM [ProfileData]");
		}

		public async Task<IEnumerable<ProfileData>> Get_Node_For_Profile (string profileName) 
		{
			return await database.QueryAsync<ProfileData>("SELECT * FROM [ProfileData] WHERE [profileName] = ?",profileName);
		}

		public async Task<IEnumerable<ProfileData>> Get_Profile_Name_Is_Open (string profileStatus) 
		{
			return await database.QueryAsync<ProfileData>("SELECT [profileName] FROM [ProfileData] WHERE [profile_status] = ?",profileStatus);
		}

		public async Task<IEnumerable<ProfileData>> Delete_Profile_By_ProfileName (string profileName) 
		{
			return await database.QueryAsync<ProfileData>("DELETE FROM [ProfileData] WHERE [profileName] = ?", profileName);
		}

		public async Task<IEnumerable<ProfileData>> Update_NameByUser_Profile () 
		{
			//return await database.QueryAsync<ProfileData>("UPDATE [ProfileData] SET [ProfileData].[profileName] = [NameByUser].[node_name_by_user] FROM [ProfileData], [NameByUser] WHERE [ProfileData].[nodeAddrOfProfile] = [NameByUser].[node_addr]");
			return await database.QueryAsync<ProfileData>("UPDATE [ProfileData] SET [NameByUserNodeOfProfile] = (SELECT node_name_by_user FROM [NameByUser] WHERE [node_addr] = [nodeAddrOfProfile])");
		}

		/**public async Task<IEnumerable<ProfileData>> Insert_ProfileData_Item (string profileName)
		{
			return await database.QueryAsync<ProfileData>("INSERT INTO [ProfileData] ([profileName]) VALUES (?)",profileName);
		}**/

		public async Task<IEnumerable<ProfileData>> Edit_ProfileData_Item (string newProfileName, string oldProfileName)
		{
			return await database.QueryAsync<ProfileData>("UPDATE [ProfileData] SET [profileName] = ? WHERE [profileName] = ?",newProfileName, oldProfileName);
		}

		public async Task<IEnumerable<ProfileData>> Get_profileName ()
		{
			return await database.QueryAsync<ProfileData>("SELECT * FROM [ProfileData]");
		}

		public async Task<IEnumerable<ProfileData>> Set_profile_Status (string profileName, string onStatus, string offStatus)
		{
			await database.QueryAsync<ProfileData> ("UPDATE [ProfileData] SET [profile_status] = ? WHERE [profileName] != ?",offStatus, profileName);
			return await database.QueryAsync<ProfileData>("UPDATE [ProfileData] SET [profile_status] = ? WHERE [profileName] = ?",onStatus, profileName);
		}

		public async Task<IEnumerable<ProfileData>> Set_profile_Status_Off (string profileName, string offStatus)
		{
			//await database.QueryAsync<ProfileData> ("UPDATE [ProfileData] SET [profile_status] = ? WHERE [profileName] != ?",offStatus, profileName);
			return await database.QueryAsync<ProfileData>("UPDATE [ProfileData] SET [profile_status] = ? WHERE [profileName] = ?",offStatus, profileName);
		}
		#endregion 
		#region RemoteData
		public async Task<IEnumerable<RemoteData>> Save_RemoteData_Item (string node_addr, string remote_button_name, string remote_username)
		{
			return await database.QueryAsync<RemoteData>("INSERT INTO [RemoteData] ([node_addr], [remote_button_name], [remote_username]) VALUES (?, ?, ?)",node_addr, remote_button_name, remote_username);
		}

		public async Task<IEnumerable<RemoteData>> Get_RemoteData_Item ()
		{
			return await database.QueryAsync<RemoteData>("SELECT * FROM [RemoteData]");
		}

		public async Task<IEnumerable<RemoteData>> Delete_RemoteData_Item ()
		{
			return await database.QueryAsync<RemoteData>("DELETE FROM [RemoteData]");
		}

		public async Task<IEnumerable<RemoteData>> Rename_RemoteData_Item (string oldName, string newName, string remote_username)
		{
			return await database.QueryAsync<RemoteData>("UPDATE [RemoteData] SET [remote_button_name] = ? WHERE [remote_button_name] = ? AND [remote_username] = ?",newName, oldName, remote_username);
		}

		public async Task<IEnumerable<RemoteData>> Delete_RemoteData_Custom_Item (string remoteName, string remoteUsername)
		{
			return await database.QueryAsync<RemoteData>("DELETE FROM [RemoteData] WHERE [remote_button_name] = ? AND [remote_username] = ?", remoteName, remoteUsername);
		}
		#endregion 

		#region Login
		public async Task<IEnumerable<Login>> Save_Login_Item (string username, string password, string flagForLogin, string lastConnectWebscoketUrl)
		{
			return await database.QueryAsync<Login>("INSERT INTO [Login] ([username], [password], [flagForLogin], [lastConnectWebscoketUrl]) VALUES (?, ?, ?, ?)",username, password, flagForLogin, lastConnectWebscoketUrl);
		}

		public async Task<IEnumerable<Login>> Update_Login_Item (string username, string password, string flagForLogin)
		{
			return await database.QueryAsync<Login>("UPDATE [Login] SET [username] = ?, [password] = ?, [flagForLogin] = ? WHERE [ID] = 0",username, password, flagForLogin);
		}

		public async Task<IEnumerable<Login>> Delete_Login_Item ()
		{
			return await database.QueryAsync<Login>("DELETE FROM [Login]");
		}

		public async Task<IEnumerable<Login>> Get_flag_Login ()
		{
			return await database.QueryAsync<Login>("SELECT * FROM [Login]");
		}

		public async Task<IEnumerable<Login>> Get_username_for_delete ()
		{
			return await database.QueryAsync<Login>("SELECT * FROM [Login] WHERE [username] != 'admin'");
		}

		public async Task<IEnumerable<Login>> Delete_username_for_delete ()
		{
			return await database.QueryAsync<Login>("DELETE FROM [Login] WHERE [username] != 'admin'");
		}

		public async Task<IEnumerable<Login>> Check_Login_Table_is_Emtry () 
		{
			return await database.QueryAsync<Login>("SELECT COUNT(*) from [Login]");
		}

		#endregion 

		#region User For delete
		public async Task<IEnumerable<LoginUsernameForDel>> Add_Login_Username_Show_For_Del (string username) 
		{
			return await database.QueryAsync<LoginUsernameForDel>("INSERT INTO [LoginUsernameForDel] ([username]) VALUES (?)", username);
		}

		public async Task<IEnumerable<LoginUsernameForDel>> Get_Login_Username_Show_For_Del () 
		{
			return await database.QueryAsync<LoginUsernameForDel>("SELECT * FROM [LoginUsernameForDel]");
		}

		public async Task<IEnumerable<LoginUsernameForDel>> Delete_Login_Username_Show_For_Del (string username) 
		{
			return await database.QueryAsync<LoginUsernameForDel>("DELETE FROM [LoginUsernameForDel] WHERE [username] = ?", username);
		}

		public async Task<IEnumerable<LoginUsernameForDel>> Delete_All_Login_Username_Show_For_Del () 
		{
			return await database.QueryAsync<LoginUsernameForDel>("DELETE FROM [LoginUsernameForDel]");
		}


		#endregion 

		#region NameByUser

		public async Task<IEnumerable<NameByUser>> Get_NameByUser_by_addr (string addr) 
		{
			return await database.QueryAsync<NameByUser>("SELECT * FROM [NameByUser] WHERE node_addr = ?", addr);
		}

		public async Task<IEnumerable<NameByUser>> Get_NameByUser_GroupBy_Addr () 
		{
			return await database.QueryAsync<NameByUser>("SELECT DISTINCT [node_addr], [node_name_by_user], [node_deviceType] FROM [NameByUser]");
		}

		public async Task<IEnumerable<NameByUser>> Get_NameByUser () 
		{
			return await database.QueryAsync<NameByUser>("SELECT * FROM [NameByUser]");
		}

		public async Task<IEnumerable<NameByUser>> Save_NameByUser (Db_allnode item, string ioName, string node_io_p, string io_value)
		{
			return await database.QueryAsync<NameByUser>("INSERT INTO [NameByUser] ([node_addr], [node_deviceType], [node_name_by_user], [node_io], [io_name_by_user], [node_io_p], [io_value]) VALUES (?, ?, ?, ?, ?, ?, ?)",item.node_addr, item.node_deviceType, item.node_addr, item.node_io, ioName, node_io_p, io_value);
		}

		public async Task<IEnumerable<NameByUser>> Update_NameByUser_ioValue (string nodeio, string addr)
		{
			return await database.QueryAsync<NameByUser>("UPDATE [NameByUser] SET [node_io] = ? WHERE [node_addr] = ?",nodeio, addr);
		}

		public async Task<IEnumerable<NameByUser>> Update_NameByUser_ioValue2 (string node_io, string io_value, string addr, string node_io_p)
		{
			return await database.QueryAsync<NameByUser>("UPDATE [NameByUser] SET [node_io] = ?, [io_value] = ? WHERE [node_addr] = ? AND [node_io_p] = ?",node_io, io_value, addr, node_io_p);
		}

		public async Task<IEnumerable<NameByUser>> Update_NameByUser_by_target_io (string ioName, string addr, string node_io_p)
		{
			return await database.QueryAsync<NameByUser>("UPDATE [NameByUser] SET [io_name_by_user] = ? WHERE [node_addr] = ? AND [node_io_p] = ?",ioName, addr, node_io_p);
		}

		public async Task<IEnumerable<NameByUser>> Update_NameByUser (string name, string addr)
		{
			return await database.QueryAsync<NameByUser>("UPDATE [NameByUser] SET [node_name_by_user] = ? WHERE [node_addr] = ?",name, addr);
		}
		#endregion

		public async Task<IEnumerable<Db_allnode>> GetItems ()
		{
			return await  database.Table<Db_allnode> ().ToListAsync ();
		}

		public async Task<IEnumerable<Db_allnode>> GetItemsNotDone ()
		{
			//lock (locker) {
			return await database.QueryAsync<Db_allnode>("SELECT * FROM [Db_allnode] GROUP BY [node_deviceType]");
			//}
		}

		public async Task<IEnumerable<Db_allnode>> GetItemGroupByDeviceType ()
		{
			return await database.QueryAsync<Db_allnode>("SELECT * FROM [Db_allnode] WHERE [node_deviceType] != 'UnknowDeviceType' GROUP BY [node_deviceType]");
		}

		public async Task<IEnumerable<Db_allnode>> GetItemByDeviceType (string deviceType)
		{
			return await database.QueryAsync<Db_allnode>("SELECT * FROM [Db_allnode] WHERE [node_deviceType] = ?", deviceType);
		}

		public async Task<IEnumerable<Db_allnode>> GetIoOfNode (string deviceAddr)
		{
			return await database.QueryAsync<Db_allnode> ("SELECT * FROM [Db_allnode] WHERE [node_addr] = ?", deviceAddr);
		}


		/**public async DeviceDatabaseTable GetItem (int id) 
		{
			//lock (locker) {
				return database.Table<DeviceDatabaseTable>().FirstOrDefault(x => x.ID == id);
			//}
		}**/

		/**public async Task<int> SaveItem (DeviceDatabaseTable item) 
		{
			//lock (locker) {
				if (item.ID != 0) {
					await database.UpdateAsync(item);
					return item.ID;
				} else {
					return await database.InsertAsync(item);
				}
			//}
		}**/

		public async Task<int> Save_DBAllNode_Item (Db_allnode item) 
		{	
			System.Diagnostics.Debug.WriteLine ("Save_DBAllNode_Item");
			return await database.InsertAsync(item);
		}

		//public async Task<int> Update_Item (DeviceDatabaseTable item) 
		//{
			//return await database.UpdateAsync(item);
		//}

		public async Task<int> Update_DBAllNode_Item (Db_allnode item) 
		{
			return await database.UpdateAsync(item);
		}

		public async Task<IEnumerable<Db_allnode>> Update_DBAllNode_All_Item (Db_allnode item) 
		{
			return await database.QueryAsync<Db_allnode>("UPDATE [Db_allnode] SET " +
				"[node_io] = ?, " +
				"[node_status] = ?, " +
				"[node_type] = ?, " +
				"[node_deviceType]  = ? " +
				"WHERE [node_addr] = ?",
				item.node_io, item.node_status, item.node_type, item.node_deviceType, item.node_addr);
		}

		public async Task<IEnumerable<Db_allnode>> Update_DBAllNode_Item2 (string node_status2, int ID2)
		{
			return await database.QueryAsync<Db_allnode>("UPDATE [Db_allnode] SET [node_status] = ? WHERE [ID] = ?",node_status2, ID2);
		}

		public async Task<IEnumerable<Db_allnode>> Update_Node_Io (string node_io, string addr)
		{
			return await database.QueryAsync<Db_allnode>("UPDATE [Db_allnode] SET [node_io] = ? WHERE [node_addr] = ?", node_io, addr);
		}

		public async Task<IEnumerable<Db_allnode>> Update_Node_Io_Node_Status (string node_io, string node_status, string addr)
		{
			//return await database.QueryAsync<Db_allnode>("UPDATE [Db_allnode] SET [node_io] = " + "'" + node_io + "', " + "[node_status] = " + "'" + node_status + "'" + " WHERE [node_addr] = " + "'" + addr + "'");
			return await database.QueryAsync<Db_allnode>("UPDATE [Db_allnode] SET [node_io] = ? , [node_status] = ? WHERE [node_addr] = ?", node_io, node_status, addr);
		}

		public async Task<IEnumerable<Db_allnode>> Update_Node_NameByUser(string name, string addr)
		{			
			return await database.QueryAsync<Db_allnode>("UPDATE Db_allnode SET name_by_user = ? WHERE node_addr = ?",name, addr);
		}

		/**public  async Task<int> DeleteItem(int id)SET NAMES UTF8
		{
			//lock (locker) {
				return await database.DeleteAsync<DeviceDatabaseTable>(id);
			//}
		}**/
	}
}

