using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Common.Interfaces;

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

	public class CValue: IObjectBase
	{
		public string Id { get; set; }
		public string CollumnId { get; set; }
		public string TableId { get; set; }
		public string RowNumb { get; set; }

		public DateTime DateValue { get; set; }
		public String StrValue { get; set; }
		public bool BoolValue { get; set; }
		public float FloatValue { get; set; }
		public float IntValue { get; set; }


		public CColumn Column { get; set; }
		public Type ValueType { get; set; }

		#region Члены IObjectBase

		public Status Status
		{
			get;
			set;
		}

		public bool Save()
		{
			throw new NotImplementedException();
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
				case TypeCode.Single:
					FloatValue = (float)data;
					break;

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
		}

		public object GetValue()
		{
			throw new NotImplementedException();
		}
	}
}
