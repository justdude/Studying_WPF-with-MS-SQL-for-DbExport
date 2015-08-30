using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Common.Interfaces;

namespace DBExport.Common.Messages
{
	public class ChangeSelected : MessageExtended
	{
		public IObjectBase SelectedItem { get; set; }
		public bool IsChange { get; set; }
	}
}
