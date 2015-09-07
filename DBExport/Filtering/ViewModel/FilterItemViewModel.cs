using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Data;
using DBExport.Common.MVVM;

namespace DBExport.Filtering.ViewModel
{
	public class FilterItemViewModel : ViewModelExtended
	{
		public FilterItemViewModel(CFilter filter)
		{
			Current = filter;
		}

		public CFilter Current { get; private set; }

		public string Name
		{
			get
			{
				return Current.Name;
			}
		}

		public string QuerySQL
		{
			get
			{
				return Current.QuerySQL;
			}
		}
	}
}
