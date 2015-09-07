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
using DbExport.Database;

namespace DBExport.Filtering.ViewModel
{
	public class SelectFilterViewModel : ViewModelExtended
	{
		private readonly RelayCommand mvSaveCommand;
		private readonly RelayCommand mvCloseCommand;
		private Action<CTable, List<string>> onFilterSelected;
		private CTable table;
		private FilterItemViewModel mvSelectedItem;
		private string mvParams;

		public SelectFilterViewModel(CTable table, Action<CTable, List<string>> onFilterSelected)
		{
			mvSaveCommand = new RelayCommand(OnSaveSelected, CanSave);
			mvCloseCommand = new RelayCommand(OnClose);

			Filters = new ObservableCollection<FilterItemViewModel>();

			this.onFilterSelected = onFilterSelected;
			this.table = table;
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
				RefreshButtons();

				RaisePropertyChanged(() => this.SelectedItem);
			}
		}

		public string Params
		{
			get
			{
				return mvParams;
			}
			set
			{
				if (mvParams == value)
					return;

				mvParams = value;
				RefreshButtons();

				RaisePropertyChanged(() => Params);
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
			return SelectedItem != null && !string.IsNullOrWhiteSpace(Params);
		}

		protected override void OnTokenChanged()
		{
			base.OnTokenChanged();

			if (string.IsNullOrWhiteSpace(Token))
				return;

			LoadData();
		}

		private void LoadData()
		{
			IEnumerable<CFilter> filters = Engine.Instance.GetFilters(table);

			foreach (CFilter item in filters)
			{
				FilterItemViewModel filterVM = new FilterItemViewModel(item);
				Filters.Add(filterVM);
			}
		}

		private void RefreshButtons()
		{
			SaveCommand.RaiseCanExecuteChanged();
		}


		private void OnSaveSelected()
		{
			try
			{
				List<string> items = null;

				var tr = CDatabase.Instance.BeginTransaction();

				SelectedItem.Current.Parameters.Add(Params);
				items = SelectedItem.Current.ExecuteQuery(tr);

				RaiseOnFilterSelected(table, items);
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

		private void RaiseOnFilterSelected(CTable table, List<string> items)
		{
			if (onFilterSelected == null)
				return;

			onFilterSelected(table, items);
		}

	}
}
