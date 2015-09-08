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
	public class DataVasedArgument
	{
		public int Count {get; set;}
		public int Current {get; set;}

		public override string ToString()
		{
			return Current + @"/" + Count;
		}
	}

	public static class CObjectBaseExtension
	{

		public static bool SaveList<T>(this IEnumerable<T> items, Status status, SqlCeTransaction tr, Action<DataVasedArgument> onDataSaving) where T : IObjectBase
		{
			DataVasedArgument savingArg = new DataVasedArgument();
			savingArg.Count = items.Count();
			bool res = true;
			int count = 0;
			try
			{
				foreach (var item in items)
				{
					if (item.Status == Status.Normal)
						continue;
					//item.Status = status;
					res &= item.Save(tr);
					count++;

					if (onDataSaving != null)
					{
						savingArg.Current = count;
						onDataSaving(savingArg);
					}
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
