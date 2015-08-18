using DbExport.Common.Interfaces;
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
		public Type CollType { get; set; }

		public Status Status
		{
			get;
			set;
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}
	}
}
