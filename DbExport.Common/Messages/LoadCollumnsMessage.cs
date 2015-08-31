using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBExport.Common.Containers;

namespace DBExport.Common.Messages
{
	public class LoadCollumnsMessage : MessageExtended
	{
		public List<CCollumnItem> Collumns { get; set; }
	}
}
