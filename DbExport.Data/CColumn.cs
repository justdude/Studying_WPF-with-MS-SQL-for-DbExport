using DbExport.Common.Interfaces;
using DbExport.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Database;
using CRM.Database;
using System.Data.SqlServerCe;

namespace DbExport.Data
{
	public class CColumn : CObjectBase
	{
		public string Name { get; set; }
		public string TableId { get; set; }

		public string CollType { get; set; }

		#region IObjectBase

		public override bool Save(SqlCeTransaction tr)
		{
			bool res = true;

			try
			{
				//save this
				switch (Status)
				{
					case Status.Added:
						this.Id = Generator.GenerateID();
						res = AddValue(this, tr);
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

		private bool AddValue(CColumn item, SqlCeTransaction tr)
		{
			string str = modSQL.InsertColumn(item);
			return CDatabase.Instance.ExecuteNonQuery(str, tr);
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

		public void SetType(Type type)
		{
			this.CollType = GetType(type);
		}

		public static string GetType(Type type)
		{ 
			if (type == null)
				return Constants.CollumnTypes.StringType;

			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					return Constants.CollumnTypes.BoolType;
				case TypeCode.DateTime:
					return Constants.CollumnTypes.DateTimeType;
				case TypeCode.Int32:
					return Constants.CollumnTypes.IntType;
				case TypeCode.String:
					return Constants.CollumnTypes.StringType;
				case TypeCode.Single:
					return Constants.CollumnTypes.FloatType;

				case TypeCode.Byte:
				case TypeCode.Char:
				case TypeCode.DBNull:
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Empty:
				case TypeCode.Int16:
				case TypeCode.Int64:
				case TypeCode.Object:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				default:
					break;
			}

			return Constants.CollumnTypes.StringType;;
		}

		public static Type GetType(string strTypeName)
		{
			if (string.IsNullOrWhiteSpace(strTypeName))
				return typeof(string);

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
