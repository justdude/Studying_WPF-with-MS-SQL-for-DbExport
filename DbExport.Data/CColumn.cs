using DbExport.Common.Interfaces;
using DbExport.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Database;
using CRM.Database;

namespace DbExport.Data
{
	public class CColumn : CObjectBase
	{
		public string Name { get; set; }
		public string TableId { get; set; }

		public string CollType { get; set; }

		#region IObjectBase

		public override bool Save()
		{
			bool res = true;

			try
			{
				//save this
				switch (Status)
				{
					case Status.Added:
						this.Id = Generator.GenerateID();
						res = AddValue(this);
						Status = Common.Interfaces.Status.Normal;
						break;
					case Status.Normal:
						break;
					case Status.Updated:
						res = UpdateValue(this);
						Status = Common.Interfaces.Status.Normal;
						break;
					case Status.Deleted:
						res = DeleteValue(this.Id);
						break;
					default:
						Status = Common.Interfaces.Status.Normal;
						break;
				}
			}
			catch (Exception ex)
			{
				res = false;
			}

			if (res)
			{
				Status = Common.Interfaces.Status.Normal;
			}

			return res;
		}

		private bool AddValue(CColumn item)
		{
			string str = modSQL.InsertColumn(item);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		private bool UpdateValue(CColumn item)
		{
			string str = modSQL.UpdateColumn(item);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		private bool DeleteValue(string id)
		{
			string str = modSQL.DeleteColumn(id);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		#endregion
		public Type GetCollType()
		{
			Type type = GetType(this.CollType);
			return type;
		}

		public static Type GetType(string strTypeName)
		{
			switch (strTypeName)
			{
				case CollumnTypes.BoolType:
					return typeof(bool);

				case CollumnTypes.DateTimeType:
					return typeof(DateTime);

				case CollumnTypes.FloatType:
					return typeof(float);

				case CollumnTypes.IntType:
					return typeof(int);

				default:
					return typeof(string);
			}
		}
	}
}
