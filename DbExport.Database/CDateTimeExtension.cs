using System;
using System.Collections.Generic;
using System.Globalization;
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

		public static bool TryParse(string source, out DateTime result)
		{
			DateTime date = DateTime.MinValue;

			string[] formats = { "yyyy", //simple year
													 "yyyy-MM-dd HH:mm:ss", //sql
													 "dd.MM.yyyy", 
													 "dd.MM.yyyy HH:mm:ss", 
													 "dd.MM.yyyy H:mm:ss" };

			IFormatProvider culture = CultureInfo.CurrentCulture;

			if (DateTime.TryParseExact(source, formats, culture, DateTimeStyles.NoCurrentDateDefault, out date))
			{
				result = date;

				return true;
			}
			else
			{
				result = date;
				return false;
			}
		}

	}
}
