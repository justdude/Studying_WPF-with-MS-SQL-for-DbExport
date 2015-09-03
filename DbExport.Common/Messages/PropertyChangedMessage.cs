using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Common.Messages
{
	public class PropertyChangedMessage: MessageExtended
	{
		public string PropName { get; set; }
	}
}
