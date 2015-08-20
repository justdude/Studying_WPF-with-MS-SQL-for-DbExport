using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Database
{
	public static class modSQL
	{
		#region Country

		//public static string SelectCountryes()
		//{
		//	return @"select * from Countryes";
		//}

		//public static string InsertCountry(Country c)
		//{
		//	string query = @"INSERT INTO Countryes(Id,CountryName) VALUES('{0}', '{1}');";
		//	return string.Format(query, c.Id, c.Name);
		//}

		//public static string UpdateCountry(Country c)
		//{
		//	string query = @"UPDATE Countryes SET City='{0}' WHERE Id='{1}';";
		//	return string.Format(query, c.Name, c.Id);
		//}

		//public static string DeleteCountry(string id)
		//{
		//	string query = string.Format(@"DELETE from Countryes WHERE Id='{0}';", id);
		//	return query;
		//}

		#endregion

		#region Tables

		public static string SelectTables()
		{
			return @"select * from Conf_Dyn_Tables";
		}

		//public static string InsertEmployes(Employee empl)
		//{
		//	string query = @"INSERT INTO Employes(Id,EmployeName,BirthDate,Salary,CountryID) VALUES('{0}', '{1}', '{2}', '{3}', '{4}');";
		//	return string.Format(query, empl.Id, empl.Name, empl.BirthDate.ToShortDateString(), empl.Salary, empl.CountryID);
		//}

		//public static string UpdateEmployes(Employee empl)
		//{
		//	string query = @"UPDATE Employes SET EmployeName='{0}',BirthDate='{1}',Salary='{2}',CountryID='{3}'" 
		//		+ " WHERE Id='{4}';";
		//	return string.Format(query, empl.Name, empl.BirthDate.ToShortDateString(), empl.Salary, empl.CountryID, empl.Id);
		//}

		//public static string DeleteEmployes(string id)
		//{
		//	string query = string.Format(@"DELETE from Employes WHERE Id='{0}';", id);
		//	return query;
		//}



		public static string InsertTable(DbExport.Data.CTable item)
		{
			throw new NotImplementedException();
		}

		internal static string UpdateTable(DbExport.Data.CTable cTable)
		{
			throw new NotImplementedException();
		}

		internal static string DeleteTable(string Id)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Collumns
		public static string SelectCollumns(string tableId)
		{
			string str = string.Format(@"select * from Conf_Dyn_Properties t where t.Id = '{0}'", tableId);
			return str;
		}

		#endregion

		public static string SelectValues(string tableId)
		{
			string str = string.Format(@"select * from Conf_Dyn_Values t where t.Id = '{0}'", tableId);
			return str;
		}
	}
}
