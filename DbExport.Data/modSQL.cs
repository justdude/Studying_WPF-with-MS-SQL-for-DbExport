using DbExport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Database;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace CRM.Database
{
	public static class modSQL
	{
		#region Tables

		public static string SelectTables()
		{
			return @"select * from Conf_Dyn_Tables";
		}

		public static string InsertTable(CTable table)
		{
			string query = @"INSERT INTO Conf_Dyn_Tables(Id, Name) VALUES('{0}', '{1}');";
			return string.Format(query, table.Id, table.Name);
		}

		public static string UpdateTable(CTable table)
		{
			string query = @"UPDATE Conf_Dyn_Tables SET Name='{0}'"
				+ " WHERE Id='{1}';";
			return string.Format(query, table.Name, table.Id);
		}

		public static string DeleteTable(string id)
		{
			string query = string.Format(@"DELETE from Conf_Dyn_Tables WHERE Id='{0}';", id);
			return query;
		}

		#endregion

		#region Values
		public static string SelectValues(string tableId)
		{
			string str = string.Format(@"select * from Conf_Dyn_Values t where t.Id = '{0}'", tableId);
			return str;
		}

		public static string SelectValues()
		{
			string str = @"select * from Conf_Dyn_Values";
			return str;
		}

		public static string DeleteValue(string id)
		{
			string str = @"DELETE FROM Conf_Dyn_Values
							Where Id='{0}'";

			return string.Format(str, id);
		}

//		public static string UpdateValue(CValue item)
//		{
//			string str = @"UPDATE Conf_Dyn_Values 
//							SET TableId ='{0}', CollId ='{1}', RowNumb ='{2}', 
//							Float ='{3}', String ='{4}', DateTime ='{5}', Bool ='{6}', Int ='{7}'
//							Where Id='{8}'";

//			return string.Format(str, item.TableId, item.CollumnId, item.RowNumb,
//								 item.FloatValue, item.StrValue, item.DateValue, item.BoolValue, item.IntValue, item.Id);
//		}

//		public static string InsertValue(CValue item)
//		{
//			string str = @"INSERT INTO Conf_Dyn_Values(Id, TableId, CollId, RowNumb, Float, String, DateTime, Bool, Int) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";

//			return string.Format(str, item.Id, item.TableId, item.CollumnId, item.RowNumb,
//								 item.FloatValue, item.StrValue,
//								 item.DateValue.ToSqlDate(),
//								 item.BoolValue, item.IntValue);
//		}

		public static void InsertValue(CValue item, SqlCeCommand cmd)
		{
			if (item == null || cmd == null)
				throw new ArgumentNullException();

			string str = @"INSERT INTO Conf_Dyn_Values(Id, TableId, CollId, RowNumb, Float, String, DateTime, Bool, Int) VALUES(@Id, @TableId, @CollId, @RowNumb, @Float, @String, @DateTime, @Bool, @Int)";
			cmd.CommandText = str;

			cmd.Parameters.AddWithValue("@Id", item.Id);
			cmd.Parameters.AddWithValue("@TableId", item.TableId);
			cmd.Parameters.AddWithValue("@CollId", item.CollumnId);
			cmd.Parameters.AddWithValue("@RowNumb", item.RowNumb);

			cmd.Parameters.AddWithValue("@Float", item.FloatValue);
			cmd.Parameters.AddWithValue("@String", item.StrValue.ToSqlObject());
			cmd.Parameters.AddWithValue("@DateTime", item.DateValue.ToSqlDate());
			cmd.Parameters.AddWithValue("@Bool", item.BoolValue);
			cmd.Parameters.AddWithValue("@Int", item.IntValue);
		}

		public static void UpdateValue(CValue item, SqlCeCommand cmd)
		{
			if (item == null || cmd == null)
				throw new ArgumentNullException();

			string str = @"UPDATE Conf_Dyn_Values 
							SET TableId =@TableId, CollId =@CollId, RowNumb =@RowNumb, 
							Float =@Float, String =@String, DateTime =@DateTime, Bool =@Bool, Int =@Int
							Where Id=@Id";

			cmd.CommandText = str;

			cmd.Parameters.AddWithValue("@Id", item.Id);
			cmd.Parameters.AddWithValue("@TableId", item.TableId);
			cmd.Parameters.AddWithValue("@CollId", item.CollumnId);
			cmd.Parameters.AddWithValue("@RowNumb", item.RowNumb);

			cmd.Parameters.AddWithValue("@Float", item.FloatValue);
			cmd.Parameters.AddWithValue("@String", item.StrValue.ToSqlObject());
			cmd.Parameters.AddWithValue("@DateTime", item.DateValue.ToSqlDate());
			cmd.Parameters.AddWithValue("@Bool", item.BoolValue);
			cmd.Parameters.AddWithValue("@Int", item.IntValue);
		}

		#endregion

		#region Columns

		public static string SelectCollumns()
		{
			string str = @"select * from Conf_Dyn_Properties";
			return str;
		}

		public static string SelectCollumns(string tableId)
		{
			string str = string.Format(@"select * from Conf_Dyn_Properties t where t.Id = '{0}'", tableId);
			return str;
		}

		public static string DeleteColumn(string id)
		{
			string str = @"DELETE FROM Conf_Dyn_Properties
							Where Id='{0}'";

			return string.Format(str, id);
		}

		public static string UpdateColumn(CColumn item)
		{
			string str = @"UPDATE Conf_Dyn_Properties(Name, CollType, TableId) 
							Set('{0}','{1}','{2}')
							Where Id='{3}'";

			return string.Format(str, item.Id, item.Name, item.CollType, item.TableId, item.Id);
		}

		public static string InsertColumn(CColumn item)
		{
			string str = @"INSERT INTO Conf_Dyn_Properties(Id, Name, CollType, TableId) 
							VALUES('{0}','{1}','{2}','{3}')";

			return string.Format(str, item.Id, item.Name, item.CollType, item.TableId, item.Id);
		}

		#endregion

		#region Filtration

		public static string GetFilters(string tableID)
		{
			string str = @"SELECT Conf_Dyn_Filter.* FROM Conf_Dyn_Filter t WHERE t.TableId ='{0}'";
			return string.Format(str, tableID);
		}

		#endregion
	}
}
