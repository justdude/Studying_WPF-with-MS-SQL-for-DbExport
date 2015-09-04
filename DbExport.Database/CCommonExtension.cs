using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Database
{
	public static class CCommonExtension
	{
		public static object ToSqlObject(this string str)
		{
			object res = string.IsNullOrWhiteSpace(str) ? (object)DBNull.Value : str;

			return res;
		}
	}
}
