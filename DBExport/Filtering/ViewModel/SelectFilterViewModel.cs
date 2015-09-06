using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Data;
using DBExport.Common.Messages;
using DBExport.Common.MVVM;
using GalaSoft.MvvmLight.Command;

namespace DBExport.Filtering.ViewModel
{
	public class SelectFilterViewModel : ViewModelExtended
	{
		private readonly RelayCommand mvSaveCommand;
		private readonly RelayCommand mvCloseCommand;
		private Action<string, List<CFilter>> onFilterSelected;
		private string tableId;
		private FilterItemViewModel mvSelectedItem;

		public SelectFilterViewModel(string tableId, Action<string, List<CFilter>> onFilterSelected)
		{
			mvSaveCommand = new RelayCommand(OnSaveSelected, CanSave);
			mvCloseCommand = new RelayCommand(OnClose);

			Filters = new ObservableCollection<FilterItemViewModel>();

			this.onFilterSelected = onFilterSelected;
			this.tableId = tableId;
		}

		public ObservableCollection<FilterItemViewModel> Filters { get; set; }
		public FilterItemViewModel SelectedItem
		{
			get
			{
				return mvSelectedItem;
			}
			set
			{
				if (mvSelectedItem == value)
					return;

				mvSelectedItem = value;

				RaisePropertyChanged(() => this.SelectedItem);
			}
		}

		public RelayCommand SaveCommand
		{
			get
			{
				return mvSaveCommand;
			}
		}

		public RelayCommand CloseCommand
		{
			get
			{
				return mvCloseCommand;
			}
		}


		private bool CanSave()
		{
			return SelectedItem!= null;
		}

		private void OnSaveSelected()
		{
			try
			{
				List<CFilter> items = new List<CFilter>();
				items.Add(SelectedItem.Current);
				RaiseOnFilterSelected(tableId, items);
			}
			catch (Exception ex)
			{
				
			}

			MessengerInstance.Send<CloseWindowMessage>(new CloseWindowMessage(), Token);
		}

		private void OnClose()
		{
			MessengerInstance.Send<CloseWindowMessage>(new CloseWindowMessage(), Token);
		}

		private void RaiseOnFilterSelected(string tableId, List<CFilter> items)
		{
			if (onFilterSelected == null)
				return;

			onFilterSelected(tableId, items);
		}

	}
}
