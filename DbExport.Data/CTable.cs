using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Database;
using DbExport.Common.Interfaces;
using DbExport.Database;

namespace DbExport.Data
{
	public class CTable : IObjectBase
	{

		public CTable()
		{
			Columns = new List<CColumn>();
			Rows = new List<CValue>();
		}

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

		public List<CColumn> Columns { get; set; }
		public List<CValue> Rows { get; set; }

		#region Члены IObjectBase

		public Status Status { get; set; }

		public bool Save()
		{
			bool res = true;

			//save this
			switch (Status)
			{
				case Status.Added:
					this.Id = Generator.GenerateID();
					res = AddTable(this);
					break;
				case Status.Normal:
					break;
				//case Status.Updated:
				//	res = UpdateTable(this);
				//	break;
				//case Status.Deleted:
				//	res = DeleteTable(this);
					//break;
				default:
					Status = Common.Interfaces.Status.Normal;
					break;
			}

			res &= Columns.SaveList(Status);
			res &= Rows.SaveList(Status);

			return true;
		}

		private bool AddTable(CTable item)
		{
			string str = modSQL.InsertTable(item);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		private bool UpdateTable(CTable item)
		{
			string str = modSQL.UpdateTable(this);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		private bool DeleteTable(string ud)
		{
			string str = modSQL.DeleteTable(Id);
			return CDatabase.Instance.ExecuteNonQuery(str);
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
