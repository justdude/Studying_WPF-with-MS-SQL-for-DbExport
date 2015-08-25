using CRM.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DbExport.Data;
using DbExport.Database;
using System.Data.SqlClient;
using DbExport.Common.Interfaces;
using System.Data.SqlServerCe;

namespace DbExport.Data
{
	public class Engine
	{
		public static Engine Instance { get; private set; }

		static Engine()
		{
			Instance = new Engine();
		}

		private static DateTime ConvertDateTime(System.Data.SqlClient.SqlDataReader reader)
		{
			return Convert.ToDateTime(reader.GetDateTime(2).ToString("dd.MM.yyyy"));
		}

		private static void InitDatetime()
		{
			CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
			ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
			Thread.CurrentThread.CurrentCulture = ci;
		}

		public IEnumerable<CTable> LoadTables()
		{
			var list = new List<CTable>();

			List<CColumn> columns = LoadCollumns().ToList();
			List<CValue> rows = LoadValues().ToList();

			var reader = CDatabase.Instance.Execute(modSQL.SelectTables());
			CTable item = null;
			try
			{
				while (reader.Read())
				{
					item = new CTable();
					item.Id = reader.GetClearStr(0);
					item.Name = reader.GetClearStr(1);
					item.Status = Status.Normal;

					FillBy(item, columns, rows);

					item.Data = CTable.ToDataTable(item);

					list.Add(item);
				}

				reader.Close();
			}
			catch(Exception ex)
			{
				if (reader != null)
					reader.Close();
			}

			return list;
		}

		public IEnumerable<CValue> LoadValues()
		{
			List<CValue> list = new List<CValue>();
			CValue item = null;
			SqlCeDataReader reader = null;
			try
			{
				reader = CDatabase.Instance.Execute(modSQL.SelectValues());

				while (reader.Read())
				{
					item = new CValue();
					item.Id = reader.GetClearStr(0);
					item.TableId = reader.GetClearStr(1);
					item.CollumnId = reader.GetClearStr(2);
					item.RowNumb = reader.GetIntSafe(3);

					try
					{
						item.FloatValue = reader.GetFloatSafe(4);
						item.StrValue = reader.GetStringSafe(5);
						item.DateValue = reader.GetDateTimeSafe(6); //reader.GetConvertedDateTime(6);
						item.BoolValue = reader.GetBoolSafe(7);
						item.IntValue = reader.GetIntSafe(8);
					}
					catch(Exception ex)
					{

					}

					item.Status = Status.Normal;

					list.Add(item);
				}

			}
			catch
			{
				if (reader != null)
					reader.Close();
			}
			finally
			{
				if (reader != null && !reader.IsClosed)
					reader.Close();
			}

			return list;
		}

		public IEnumerable<CColumn> LoadCollumns()
		{
			List<CColumn> list = new List<CColumn>();
			CColumn item = null;
			SqlCeDataReader reader = null;
			try
			{
				reader = CDatabase.Instance.Execute(modSQL.SelectCollumns());

				while (reader.Read())
				{
					item = new CColumn();
					item.Id = reader.GetClearStr(0);
					item.Name = reader.GetClearStr(1);
					item.CollType = reader.GetClearStr(2);
					item.TableId = reader.GetClearStr(3);
					item.Status = Status.Normal;

					list.Add(item);
				}

			}
			catch (Exception ex)
			{

			}
			finally
			{
				if (reader != null && !reader.IsClosed)
					reader.Close();
			}

			return list;
		}

		private void FillBy(CTable table, List<CColumn> columns, List<CValue> rows)
		{
			if (table == null)
				return;

			table.Columns = columns.Where(p => p.TableId == table.Id).ToList();
			table.Rows = rows.Where(p => p.TableId == table.Id).ToList();

			foreach (var item in table.Rows)
			{
				item.Column = table.Columns.FirstOrDefault(p => p.Id == item.CollumnId);
				if (item.Column == null)
					continue;
				item.ValueType = item.Column.GetCollType();
			}
		}

	}
}
