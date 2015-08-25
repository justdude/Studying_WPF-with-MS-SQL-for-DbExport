using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Database
{
	public static class CDateTimeExtension
	{
		const string MinSQLDateTime = "01.01.1753 00:00:00";

		public static string ToSqlDate(this DateTime d)
		{
			if (d == DateTime.MinValue)
				return MinSQLDateTime;

			return d.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
		}

	}
}
