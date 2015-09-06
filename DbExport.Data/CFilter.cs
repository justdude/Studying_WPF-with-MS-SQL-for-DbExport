using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Data
{
	public class CFilter : CObjectBase
	{
		public string TableId { get; set; }
		public string QuerySQL { get; set; }
		public string Name { get; set; }

		public List<string> ExecuteQuery(SqlCeTransaction tr)
		{
			List<string> ids = new List<string>();


			return ids;
		}
	}
}
