using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Database;

namespace DbExport.Data
{
	public class CFilter : CObjectBase
	{
		public const string ParamNameConst = @"Value";

		public string TableId { get; set; }
		public string QuerySQL { get; set; }
		public string Name { get; set; }
		public List<object> Parameters { get; set;}

		public CFilter()
		{
			Parameters = new List<object>();
		}

		public List<string> ExecuteQuery(SqlCeTransaction tr)
		{
			List<string> ids = new List<string>();

			if (string.IsNullOrWhiteSpace(QuerySQL) && Parameters.Count > 0)
				return ids;

			try
			{
				SqlCeCommand command = new SqlCeCommand(QuerySQL);
				command.Parameters.AddWithValue(ParamNameConst, Parameters.FirstOrDefault());

				SqlCeDataReader reader = Database.CDatabase.Instance.Execute(command, tr);

				while (reader.Read())
				{
					string str = reader.GetClearStr(0);

					if (string.IsNullOrWhiteSpace(str))
						continue;

					ids.Add(str);
				}

			}
			catch(Exception ex)
			{
			
			}

			return ids;
		}



	}
}
