using DbExport.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Common.Containers
{
	public class CCollumnItem
	{
		public string Name { get; set; }
		public Type ItemType { get; set; }
		public IObjectBase Coll { get; set; }
	}
}
