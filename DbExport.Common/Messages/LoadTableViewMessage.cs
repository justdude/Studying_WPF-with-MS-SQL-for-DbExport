using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Common.Messages
{
	public class LoadTableViewMessage : MessageExtended
	{
		public DataTable Data { get; set; }
	}
}
