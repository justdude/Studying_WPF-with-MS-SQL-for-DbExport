using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Common.Interfaces;

namespace DbExport.Data
{
	public class CTable : IObjectBase
	{
		public string Name
		{
			get
			{
				return Data.TableName;
			}
			set
			{
				Data.TableName = value;
			}
		}
		public string Id { get; set; }

		public DataTable Data { get; set; }
		public List<CColumn> Columns = new List<CColumn>();

		#region Члены IObjectBase

		public Status Status { get; set; }

		public bool Save()
		{
			return true;
		}

		#endregion

		#region Table init
		public static CTable Create(DataTable data)
		{
			CTable table = new CTable();
			table.Id = Database.Generator.GenerateID();
			table.Data = data;

			return table;
		}
		#endregion
	}
}
