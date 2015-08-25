using DbExport.Data;
using DbExport.Database;
using DBExport.Common.Messages;
using DBExport.Common.MVVM;
using DBExport.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
//using Microsoft.Practices.Prism.Commands;
//using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBExport.Settings.ViewModel
{
	public class TableSettingViewModel : ViewModelExtended
	{
		private readonly RelayCommand mvSaveCommand;
		private readonly RelayCommand mvCloseCommand;
		private Func<object, bool> modCheckIsRightCollumn;
		private CTable modTable;
		private string mvCurrentSeparator;

		public TableSettingViewModel(CTable table, Func<object, bool> checkIsRightCollumn)
		{
			mvSaveCommand = new RelayCommand(OnSaveSelected, CanSave);
			mvCloseCommand = new RelayCommand(OnClose);

			Tables = new ObservableCollection<TableSettingItemViewModel>();
			DataTypes = new List<Type>()
			{
				typeof(string),
				typeof(double),
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

			NumberDecimalSeparator = ".";
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

		public string NumberDecimalSeparator
		{
			get
			{
				return mvCurrentSeparator;
			}
			set
			{
				if (value == mvCurrentSeparator)
					return;

				mvCurrentSeparator = value;

				this.RaisePropertyChanged(() => this.NumberDecimalSeparator);
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
				SetCultureNumbSeparator(NumberDecimalSeparator);

				Dictionary<string, Type> dict = new Dictionary<string, Type>();

				foreach (var item in Tables)
				{
					if (dict.ContainsKey(item.Name))
						continue;

					dict.Add(item.Name, item.CurrentType);
				}
			
				modTable.Data = CDataTableHelper.ConvertTableType(modTable.Data, dict);
			}
			catch (Exception ex)
			{
				
			}

			MessengerInstance.Send<CloseWindowMessage>(new CloseWindowMessage(), Token);
		}

		private void SetCultureNumbSeparator(string numberDecimalSeparator)
		{
			var cultureInfo = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
			cultureInfo.NumberFormat.NumberDecimalSeparator = numberDecimalSeparator;
			cultureInfo.NumberFormat.CurrencyDecimalSeparator = numberDecimalSeparator;
			Thread.CurrentThread.CurrentCulture = cultureInfo;
		}

		private void OnClose()
		{
			MessengerInstance.Send<CloseWindowMessage>(new CloseWindowMessage(), Token);
		}

	}
}
