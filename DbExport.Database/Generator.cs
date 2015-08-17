using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Database
{
	public static class Generator
	{
		public static string GenerateID()
		{
			//System.Threading.Thread.Sleep(1);
			return DateTime.Now.GetHashCode().ToString("x");
		}
	}
}
