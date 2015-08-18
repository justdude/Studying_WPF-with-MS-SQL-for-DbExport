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

namespace DbExport.Data
{
	public class Engine
	{
		public static Engine Instance { get; private set; }

		static Engine()
		{
			Instance = new Engine();
		}

		//public IEnumerable<Employee> LoadEmployes()
		//{
		//	var list = new List<Employee>();

		//	var reader = CDatabase.Instance.Execute(modSQL.SelectEmployes());

		//	InitDatetime();

		//	Employee employee = null;
		//	try
		//	{
		//		while (reader.Read())
		//		{
		//			employee = new Employee();
		//			employee.Id = reader.GetString(0);
		//			employee.Name = reader.GetString(1);
		//			employee.BirthDate = ConvertDateTime(reader);
		//			employee.Salary = (float)reader.GetDouble(3);
		//			employee.CountryID = reader.GetString(4);
		//			employee.Status = Common.Interfaces.Status.Normal;
		//			list.Add(employee);
		//		}
		//		reader.Close();
		//	}
		//	catch(Exception ex)
		//	{
		//		if (reader != null)
		//			reader.Close();
		//	}

		//	//#region can remove
		//	//if (list.Count == 0)
		//	//{
		//	//	for (int i = 0; i < 5; i++)
		//	//	{
		//	//		list.Add(Employee.CreateRand());
		//	//	}
		//	//}
		//	//#endregion

		//	return list;
		//}

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
			//#region can remove

			//string[] cc = new string[] { "Ukraine", "USA", "Canada", "German", "Spain", "Poland" };

			//#endregion

			var list = new List<CTable>();

			var reader = CDatabase.Instance.Execute(modSQL.SelectTables());

			List<CColumn> columns = LoadCollumns().ToList();
			List<CValue> rows = LoadValues().ToList();

			CTable item = null;
			try
			{
				while (reader.Read())
				{
					item = new CTable();
					item.Id = reader.GetString(0);
					item.Name = reader.GetString(1);
					item.Status = Status.Normal;

					FillBy(item, columns, rows);

					list.Add(item);
				}

				reader.Close();
			}
			catch
			{
				if (reader != null)
					reader.Close();
			}

			//#region can remove
			//if (list.Count == 0)
			//{

			//	for (int i = 0; i < cc.Length; i++)
			//	{
			//		list.Add(new Country() { Id = i.ToString(), Name = cc[i] });
			//	}
			//}
			//#endregion

			return list;
		}

		public IEnumerable<CValue> LoadValues()
		{
			List<CValue> list = new List<CValue>();
			CValue item = null;
			SqlDataReader reader = null;
			try
			{
				reader = CDatabase.Instance.Execute(modSQL.SelectTables());

				while (reader.Read())
				{
					item = new CValue();
					item.Id = reader.GetString(0);
					item.BoolValue = reader.GetBoolean(1);
					item.DateValue = reader.GetDateTime(2);
					item.FloatValue = reader.GetFloat(2);
					item.StrValue = reader.GetString(2);
					item.CollumnId = reader.GetString(9);
					item.TableId = reader.GetString(10);
					item.Status = Status.Normal;

					list.Add(item);
				}

			}
			catch
			{
				if (reader != null)
					reader.Close();
			}

			return list;
		}

		public IEnumerable<CColumn> LoadCollumns()
		{
			List<CColumn> list = new List<CColumn>();
			CColumn item = null;
			SqlDataReader reader = null;
			try
			{
				reader = CDatabase.Instance.Execute(modSQL.SelectTables());

				while (reader.Read())
				{
					item = new CColumn();
					item.Id = reader.GetString(0);
					item.Name = reader.GetString(1);
					item.CollType = reader.GetString(2);
					item.Status = Status.Normal;

					list.Add(item);
				}

			}
			catch
			{
				if (reader != null)
					reader.Close();
			}

			return list;
		}

		private void FillBy(CTable table, List<CColumn> columns, List<CValue> rows)
		{
			if (table == null)
				return;

			table.Columns = columns.Where(p=>p.TableId == table.Id).ToList();
			table.Rows = rows.Where(p=>p.TableId == table.Id).ToList();

			foreach (var item in table.Rows)
			{
				item.Column = table.Columns.FirstOrDefault(p => p.Id == item.CollumnId);
			}
		}

	}
}
