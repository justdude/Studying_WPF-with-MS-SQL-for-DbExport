using DbExport.Data;
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
				typeof(float),
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

		private static DataTable ConvertTableType(DataTable dt, Dictionary<string, Type> types)
		{
			DataTable newDt = dt.Clone();
			//convert all columns' datatype

			newDt.TableName = dt.TableName;

			foreach (DataColumn dc in newDt.Columns)
			{
				if (types.ContainsKey(dc.ColumnName))
				{
					dc.DataType = types[dc.ColumnName];
				}
			}

			//import data from original table
			//foreach (DataRow dr in dt.Rows)
			//{
			//	newDt.ImportRow(dr);
			//}

			newDt.Load(dt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges, ErrorHandler);

			dt.Dispose();
			return newDt;
		}

		private static void ErrorHandler(object sender, FillErrorEventArgs e)
		{
			
			e.Continue = true;
		}

		private void OnSaveSelected()
		{
			try
			{
				var cultureInfo = Thread.CurrentThread.CurrentCulture;
				cultureInfo.NumberFormat.NumberDecimalSeparator = NumberDecimalSeparator;

				//DataTable newTable = new DataTable(modTable.Name);
				//foreach (DataColumn item in modTable.Data.Columns)
				//{
				//	var tableSetting = Tables.FirstOrDefault(p => p.Name == item.ColumnName);

				//	if (tableSetting == null)
				//		continue;

				//	newTable.DataType = tableSetting.CurrentType;

				//}
				Dictionary<string, Type> dict = new Dictionary<string, Type>();

				foreach (var item in Tables)
				{
					if (dict.ContainsKey(item.Name))
						continue;

					dict.Add(item.Name, item.CurrentType);
				}

				modTable.Data = ConvertTableType(modTable.Data, dict);
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

	}
}
