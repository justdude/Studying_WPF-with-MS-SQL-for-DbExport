using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Database
{
	public class CDatabaseManager
	{
		private SqlCeConnection modConnection;

		//public static string ConnectionString = @"Data Source=C:\Users\ialbantov.LIZDEVNTD\Documents\DbExport\DbExport.Database\DbEData.sdf";
		public static string ConnectionString = @"Data Source={0}";
		public const string DbPathNoteBook = @"D:\Projects\DbExport\DbExport.Database\DbEData.sdf";
		public static string DbPathWork = @"C:\Users\ialbantov.LIZDEVNTD\Documents\DbExport\DbExport.Database\DbEData.sdf";

		public CDatabaseManager()
		{
		}

		public SqlCeDataReader Execute(string command)
		{
			SqlCeCommand cmd = null;
			SqlCeDataReader reader = null;

			try
			{
				OpenIfClosed();

				cmd = new SqlCeCommand(command, modConnection);
				
				reader = cmd.ExecuteReader();
				return reader;
			}
			catch(Exception ex)
			{
				if (reader != null)
					reader.Close();

				Close();
			}
			return reader;
		}

		public SqlCeDataReader Execute(SqlCeCommand cmd, SqlCeTransaction tr)
		{
			SqlCeDataReader reader = null;

			try
			{
				OpenIfClosed();

				cmd.Connection = modConnection;
				cmd.Transaction = tr;

				reader = cmd.ExecuteReader();
				return reader;
			}
			catch (Exception ex)
			{
			}
			return reader;
		}

		private void OpenIfClosed()
		{
			if (modConnection.State == System.Data.ConnectionState.Closed)
				modConnection.Open();
		}

		public void Close()
		{
			if (modConnection == null)
				return;
			try
			{
				modConnection.Close();
			}
			catch
			{}
		}


		public SqlCeTransaction BeginTransaction()
		{
			OpenIfClosed();

			if (modConnection == null)
				return null;

			return modConnection.BeginTransaction();
		}

		[Obsolete("Use method with SQLCeCommand", false)]
		public bool ExecuteNonQuery(string command)
		{
			SqlCeCommand cmd = null;
			int res = 0;

			try
			{
				if (modConnection.State == System.Data.ConnectionState.Closed)
					modConnection.Open();
				cmd = new SqlCeCommand(command, modConnection);

				res = cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{

			}
			finally
			{
				if (modConnection!=null)
					modConnection.Close();
			}
			return res == 1;
		}

		[Obsolete("Use method with SQLCeCommand", false)]
		public bool ExecuteNonQuery(string command, SqlCeTransaction tr)
		{
			if (tr == null)
				throw new ArgumentNullException();

			SqlCeCommand cmd = null;
			int res = 0;

			try
			{
				if (modConnection.State == System.Data.ConnectionState.Closed)
					modConnection.Open();
				cmd = new SqlCeCommand(command, modConnection, tr);

				res = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{

			}
			finally
			{

			}
			return res == 1;
		}

		public bool ExecuteNonQuery(SqlCeCommand cmd)
		{
			if (cmd == null)
				throw new ArgumentNullException();

			int res = 0;

			try
			{
				if (modConnection.State == System.Data.ConnectionState.Closed)
					modConnection.Open();

				cmd.Connection = modConnection;

				res = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{

			}
			finally
			{

			}
			return res == 1;
		}

		public bool ExecuteNonQuery(SqlCeCommand cmd, SqlCeTransaction tr)
		{
			if (tr == null || cmd == null)
				throw new ArgumentNullException();

			int res = 0;

			try
			{
				if (modConnection.State == System.Data.ConnectionState.Closed)
					modConnection.Open();

				cmd.Connection = modConnection;
				cmd.Transaction = tr;

				res = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{

			}
			finally
			{

			}
			return res == 1;
		}

		public bool TryOpenConnection(string path)
		{
			if (modConnection != null)
				return true;

			bool res = false;

			try
			{
				string connString = string.Format(ConnectionString, path);
				modConnection = new SqlCeConnection(connString);
			}
			catch (Exception ex)
			{
				res = false;
			}

			return res;
		}
	}
}
