using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Database
{
	public static class CReaderExtension
	{
		public static string GetClearStr(this SqlDataReader reader, int colIndex)
		{
			string str = string.Empty;
			if (!reader.IsDBNull(colIndex))
			{
				str = reader.GetString(colIndex);
				
				if (string.IsNullOrWhiteSpace(str))
					return str;

				str = str.Trim();
			}

			return str;
		}

		public static string GetStringSafe(this SqlDataReader reader, int colIndex)
		{
			if (!reader.IsDBNull(colIndex))
				return reader.GetString(colIndex);
			else
				return string.Empty;
		}

		public static float GetFloatSafe(this SqlDataReader reader, int colIndex)
		{
			if (!reader.IsDBNull(colIndex))
				return reader.GetFloat(colIndex);
			else
				return float.NaN;
		}

		public static int GetIntSafe(this SqlDataReader reader, int colIndex)
		{
			if (!reader.IsDBNull(colIndex))
				return reader.GetInt32(colIndex);
			else
				return int.MinValue;
		}

		public static bool GetBoolSafe(this SqlDataReader reader, int colIndex)
		{
			if (!reader.IsDBNull(colIndex))
				return reader.GetBoolean(colIndex);
			else
				return false;
		}

		public static DateTime GetDateTimeSafe(this SqlDataReader reader, int colIndex)
		{
			if (!reader.IsDBNull(colIndex))
				return reader.GetDateTime(colIndex);
			else
				return DateTime.MinValue;
		}

	}
}
