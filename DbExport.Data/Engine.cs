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

			//var reader = CDatabase.Instance.Execute(modSQL.SelectTables());

			//CTable item = null;
			//try
			//{
			//	while(reader.Read())
			//	{
			//		item = new CTable();
			//		item.Id = reader.GetString(0);
			//		item.Name = reader.GetString(1);
			//		item.Status = Common.Interfaces.Status.Normal;
			//		list.Add(item);
			//	}
			//	reader.Close();
			//}
			//catch
			//{ 
			//	if (reader != null)
			//		reader.Close();
			//}

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

	}
}
