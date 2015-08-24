using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Common.Interfaces;

namespace DbExport.Data
{
	public static class CObjectBaseExtension
	{

		public static bool SaveList<T>(this IEnumerable<T> items, Status status, SqlCeTransaction tr) where T : IObjectBase
		{ 
			bool res = true;
			try
			{
				foreach (var item in items)
				{
					item.Status = status;
					res &= item.Save(tr);
				}
			}
			catch (Exception)
			{
				res = false;
			}
			return res;
		}

		public static DateTime GetConvertedDateTime(this SqlDataReader reader, int n)
		{
			return Convert.ToDateTime(reader.GetDateTime(n).ToString("dd.MM.yyyy"));
		}

	}
}
