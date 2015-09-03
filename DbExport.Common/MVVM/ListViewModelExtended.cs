using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Common.MVVM
{
	public class ListViewModelExtended<T>: ViewModelExtended where T : ViewModelExtended
	{
		public ObservableCollection<T> SelectedItems { get; set; }
	}
}
