using DbExport.Common.Interfaces;
using DbExport.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Data
{
	public class CColumn : IObjectBase
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string TableId { get; set; }

		public string CollType { get; set; }

		public Status Status
		{
			get;
			set;
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}

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
