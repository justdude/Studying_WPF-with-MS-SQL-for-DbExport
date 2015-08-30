using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Common.Interfaces;

namespace DBExport.Common.Messages
{
	public class DeleteSelected : MessageExtended
	{
		public List<IObjectBase> SelectedItems { get; set; }
	}
}
