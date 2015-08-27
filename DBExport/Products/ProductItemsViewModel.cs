using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBExport.Common.MVVM;
using GalaSoft.MvvmLight;

namespace DBExport.Products
{
	public class ProductItemsViewModel : ViewModelExtended
	{
		private DbExport.Data.CTable table;

		public ProductItemsViewModel(DbExport.Data.CTable table)
		{
			// TODO: Complete member initialization
			this.table = table;
		}
	}
}
