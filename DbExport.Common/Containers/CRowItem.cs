using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Common.Containers
{
	public class CRowItem
	{
		public string Name { get; set; }
		public Type ValueType { get; set; }
		public object Value { get; set; }

	}
}
