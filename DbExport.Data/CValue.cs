using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Database;
using DbExport.Common.Interfaces;
using DbExport.Data.Constants;
using DbExport.Database;
using DBExport.Common.Interfaces;

namespace DbExport.Data
{

	public enum CollumnType
	{
		String,
		Bool,
		Int,
		Float,
		DateTime
	}

	public class CValue : CObjectBase, IDataValue
	{
		public string Id { get; set; }
		public string CollumnId { get; set; }
		public string TableId { get; set; }
		public int RowNumb { get; set; }

		public DateTime DateValue { get; set; }
		public String StrValue { get; set; }
		public bool BoolValue { get; set; }
		public double FloatValue { get; set; }
		public float IntValue { get; set; }


		public CColumn Column { get; set; }
		public Type ValueType { get; set; }

		public string CollumnName
		{
			get
			{ 
				string str = string.Empty;

				if (Column != null)
				{
					str = Column.Name;
				}

				return str;
			}
		}
		#region Члены IObjectBase

		public override bool Save()
		{
			bool res = true;

			try
			{
				//save this
				switch (Status)
				{
					case Status.Added:
						this.Id = Generator.GenerateID(CConstants.ROW);
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

		public override bool Save(SqlCeTransaction tr)
		{
			bool res = true;

			try
			{
				//save this
				switch (Status)
				{
					case Status.Added:
						if (string.IsNullOrWhiteSpace(Id))
							this.Id = Generator.GenerateID(CConstants.ROW);
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

		private bool AddValue(CValue item, SqlCeTransaction tr)
		{
			string str = modSQL.InsertValue(item);
			return CDatabase.Instance.ExecuteNonQuery(str, tr);
		}


		private bool AddValue(CValue item)
		{
			string str = modSQL.InsertValue(item);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		private bool UpdateValue(CValue item)
		{
			string str = modSQL.UpdateValue(item);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		private bool DeleteValue(string id)
		{
			string str = modSQL.DeleteValue(id);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		#endregion

		public void SetValue(object p, CColumn coll)
		{
			CollumnId = coll.Id;
		}

		//public static ToCollType(DateTime time)
		//{
		//	CollumnType type = CollumnType.String;

			

		//}

		public void SetValue(object data)
		{
			if (data == null || data == DBNull.Value)
				return;

			var type = data.GetType();
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					BoolValue = (bool)data;
					break;
				case TypeCode.DateTime:
					DateValue = (DateTime)data;
					break;
				case TypeCode.Int32:
					IntValue = (int)data;
					break;
				case TypeCode.String:
					StrValue = (string)data;
					break;
				case TypeCode.Double:
					FloatValue = (double)data;
					break;

				case TypeCode.Byte:
				case TypeCode.Char:
				case TypeCode.DBNull:
				case TypeCode.Decimal:
				case TypeCode.Single:
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
		}

		public object GetValue()
		{
			if (this.ValueType == null)
				return null;

			switch (Type.GetTypeCode(ValueType))
			{
				case TypeCode.Boolean:
					return BoolValue;
				case TypeCode.DateTime:
					return DateValue;
				case TypeCode.Int32:
					return IntValue;
				case TypeCode.String:
					return StrValue;
				case TypeCode.Double:
					return FloatValue;

				case TypeCode.Byte:
				case TypeCode.Char:
				case TypeCode.DBNull:
				case TypeCode.Decimal:
				case TypeCode.Single:
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
			return null;
		}

		public override string ToString()
		{
			object data = GetValue();

			if (data == null)
				return string.Empty;

			return data.ToString();
		}

	}
}
