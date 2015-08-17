using DbExport.Data;
using DBExport.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
//using Microsoft.Practices.Prism.Commands;
//using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Settings.ViewModel
{
	public class TableSettingViewModel : ViewModelBase
	{
		private readonly RelayCommand mvSaveCommand;
		private readonly RelayCommand mvCloseCommand;
		private Func<object, bool> modCheckIsRightCollumn;
		private CTable modTable;

		public TableSettingViewModel(CTable table, Func<object, bool> checkIsRightCollumn)
		{
			mvSaveCommand = new RelayCommand(OnSaveSelected, CanSave);
			mvCloseCommand = new RelayCommand(OnClose);

			Tables = new ObservableCollection<TableSettingItemViewModel>();
			DataTypes = new List<Type>()
			{
				typeof(string),
				typeof(float),
				typeof(int),
				typeof(DateTime),
				typeof(bool)
			};

			this.modCheckIsRightCollumn = checkIsRightCollumn;
			this.modTable = table;

			foreach (DataColumn item in table.Data.Columns)
			{
				var tableSetting = new TableSettingItemViewModel()
				{
					Name = item.ColumnName,
					CurrentType = item.DataType
				};
				Tables.Add(tableSetting);
			}

			//eventAggregator.GetEvent<StateChangedEvent>().Subscribe(OnDataChanged);
			//eventAggregator.GetEvent<ItemChangedEvent>().Subscribe(OnSelectedChanged, ThreadOption.PublisherThread, true, Filter);
		}

		public ObservableCollection<TableSettingItemViewModel> Tables { get; set; }
		public List<Type> DataTypes { get; set; }

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
			return Tables.Any(p => p.IsHasErrors) == false;
		}

		private void OnSaveSelected()
		{
			try
			{
				DataTable newTable = new DataTable(modTable.Name);
				foreach (DataColumn item in modTable.Data.Columns)
				{
					var tableSetting = Tables.FirstOrDefault(p => p.Name == item.ColumnName);
					
					if (tableSetting == null)
						continue;

					item.DataType = tableSetting.CurrentType;
				}
			}
			catch (Exception ex)
			{

			}

			//if (SelectedTable.Current.Status == Status.Normal)
			//{
			//	SelectedTable.Current.Status = Status.Updated;
			//}

			////SelectedTable.Current.CountryID = SelectedCountry.Current.Id;
			//SelectedTable.Current.Save();
			//SelectedTable = null;

			//RaiseRefresh();
		}

		private void OnClose()
		{
		}

	}
}
