using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Database
{
	public class CDatabase
	{
		public static CDatabaseManager Instance { get; private set; }

		static CDatabase()
		{
			Instance = new CDatabaseManager();
		}

	}
}
