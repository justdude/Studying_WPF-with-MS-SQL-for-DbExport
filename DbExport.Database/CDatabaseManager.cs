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
		public static string ConnectionString = @"Data Source=D:\Projects\DbExport\DbExport.Database\DbEData.sdf";
		//public static string ConnectionString = @"Data Source=(localdb)\ProjectsV12;Initial Catalog=DbExportData;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";

		public CDatabaseManager()
		{
			modConnection = new SqlCeConnection(ConnectionString);
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

		public SqlCeDataReader ExecuteBG(string command)
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

	}
}
