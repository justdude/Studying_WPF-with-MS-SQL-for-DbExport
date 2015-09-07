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

		protected override void OnTokenChanged()
		{
			base.OnTokenChanged();
			//SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
		}

		//void SelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		//{
		//	switch (e.Action)
		//	{
		//		case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
		//			break;
		//		case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
		//			break;
		//		case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
		//			break;
		//		case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
		//			break;
		//		case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
		//			break;
		//		default:
		//			break;
		//	}
		//}

	}
}
