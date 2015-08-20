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
	}
}
