using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBExport.Common.Containers;

namespace DBExport.Common.Messages
{
	public class LoadRowsMessage : MessageExtended
	{
		public List<CRowItemViewModel> Rows { get; set; }
	}
}
