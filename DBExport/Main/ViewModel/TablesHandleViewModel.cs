using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using DbExport.Common;
using DbExport.Common.Interfaces;
using DbExport.CSV;
using DbExport.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using DBExport.Helpers;
using DBExport.Common.MVVM;
using System.Windows.Threading;
using System.Threading;
using System.Windows;
using DBExport.Common.Messages;
//using Microsoft.Practices.Prism.Commands;
//using Microsoft.Practices.Prism.Mvvm;

namespace DBExport.Main.ViewModel
{
	public class TablesHandleViewModel : ViewModelExtended
	{
		private readonly RelayCommand mvAddCommand;
		private readonly RelayCommand mvEditCommand;
		private readonly RelayCommand mvDeleteCommand;
		private readonly RelayCommand mvSaveCommand;
		private readonly RelayCommand mvRefreshCommand;
		private readonly RelayCommand mvCloseCommand;
		private readonly RelayCommand mvSettingShowCommand;
		private readonly RelayCommand mvSetMergeCommand;

		private bool mvIsBlocked;
		private bool mvIsHasError;
		private TableViewModel mvSelectedTable;
		private bool mvIsMerge;
		private bool mvIsLoading;
		private bool mvIsAppenedSuccessfully;

		public TablesHandleViewModel()
		{
			Tables = new ObservableCollection<TableViewModel>();

			mvAddCommand = new RelayCommand(OnAdd, CanAdd);
			mvEditCommand = new RelayCommand(OnEdit, CanEdit);
			mvDeleteCommand = new RelayCommand(OnDeleteSelected, CanDelete);
			mvSaveCommand = new RelayCommand(OnSaveSelected, CanSave);
			mvRefreshCommand = new RelayCommand(OnRefresh, CanRefresh);
			mvCloseCommand = new RelayCommand(OnClose);
			mvSettingShowCommand = new RelayCommand(SettingShow, CanShowSettings);
			mvSetMergeCommand = new RelayCommand(SetMerge, CanMerge);
			//eventAggregator.GetEvent<StateChangedEvent>().Subscribe(OnDataChanged);
			//eventAggregator.GetEvent<ItemChangedEvent>().Subscribe(OnSelectedChanged, ThreadOption.PublisherThread, true, Filter);
			OnDispatcherChanged += OnDispatcherChange;
		}

		private void OnDispatcherChange(System.Windows.Threading.Dispatcher obj)
		{
			LoadData();
		}

		public ObservableCollection<TableViewModel> Tables { get; set; }


		public TableViewModel SelectedTable
		{
			get
			{
				return mvSelectedTable;
			}
			set
			{
				if (mvSelectedTable == value)
					return;

				mvSelectedTable = value;
				OnSelectedChanged(value);

				this.RaisePropertyChanged(() => this.SelectedTable);
				this.RaisePropertyChanged(() => this.SelectedTableView);
			}
		}

		public DataView SelectedTableView
		{
			get
			{
				if (!IsSelected || !SelectedTable.IsExist)
					return null;

				return GetItems(SelectedTable.Current.Data, 0, 20).DefaultView;
			}
		}

		public bool IsLoading
		{
			get
			{
				return mvIsLoading;
			}
			set
			{
				if (mvIsLoading == value)
					return;

				mvIsLoading = value;

				RaisePropertyChanged(() => this.IsLoading);
			}
		}

		public bool IsCreate
		{
			get
			{
				return State == enFormState.Create;
			}
		}
		public enFormState State { get; set; }

		public DataTable GetItems(DataTable table, int offset, int count)
		{
			DataTable tableRes = new DataTable();

			tableRes.TableName = table.TableName;
			//tableRes.Columns.AddRange(table.Columns.Cast<DataColumn>().ToArray());

			foreach (DataColumn coll in table.Columns)
			{
				string name = coll.ColumnName;
				Type type = coll.DataType;
				DataColumn newColl = new DataColumn(name, type);
				tableRes.Columns.Add(newColl);
			}

			int start = offset;
			int length = Math.Min(table.Rows.Count, start + count);

			for (int i = start; i < length; i++)
			{
				object[] data = table.Rows[i].ItemArray;
				tableRes.Rows.Add(data);
			}
			return tableRes;
		}

		public string SelectedItemName
		{
			get
			{
				if (IsSelected == false)
					return string.Empty;

				return SelectedTable.Name;
			}
			set
			{
				if (!IsSelected || SelectedTable.Name == value)
					return;

				SelectedTable.Name = value;

				RaiseRefresh();

				this.RaisePropertyChanged(() => this.SelectedItemName);
			}
		}

		public bool IsEnabled
		{
			get
			{
				return mvIsBlocked;
			}
			set
			{
				if (mvIsBlocked == value)
					return;

				mvIsBlocked = value;

				this.RaisePropertyChanged(() => this.IsEnabled);
			}
		}

		public bool IsMerge
		{
			get
			{
				return mvIsMerge;
			}
			set
			{
				if (mvIsMerge == value)
					return;

				mvIsMerge = value;

				this.RaisePropertyChanged(() => this.IsMerge);
			}
		}


		//public bool IsChanged
		//{
		//	get
		//	{
		//		return SelectedTable != null && SelectedTable.IsChanged;
		//	}
		//}

		public bool IsHasError
		{
			get
			{
				return mvIsHasError;
			}
			set
			{
				if (mvIsHasError == value)
					return;

				mvIsHasError = value;

				RefreshCommands();

				this.RaisePropertyChanged(() => this.IsHasError);
			}
		}

		public bool IsSelected
		{
			get
			{
				return SelectedTable != null && SelectedTable.Current != null;
			}
		}

		private void RaisePropertyesChanged()
		{
			if (IsSelected)
				SelectedTable.RaisePropertyesChanged();

			this.RaisePropertyChanged(() => this.IsHasError);
			this.RaisePropertyChanged(() => this.IsSelected);
			this.RaisePropertyChanged(() => this.IsEnabled);
		}

		#region Члены IDataButtons

		public RelayCommand AddCommand
		{
			get
			{
				return mvAddCommand;
			}
		}

		public RelayCommand EditCommand
		{
			get
			{
				return mvEditCommand;
			}
		}

		public RelayCommand DeleteCommand
		{
			get
			{
				return mvDeleteCommand;
			}
		}

		public RelayCommand SaveCommand
		{
			get
			{
				return mvSaveCommand;
			}
		}

		public RelayCommand SettingShowCommand
		{
			get
			{
				return mvSettingShowCommand;
			}
		}

		public RelayCommand RefreshCommand
		{
			get
			{
				return mvRefreshCommand;
			}
		}

		public RelayCommand CloseCommand
		{
			get
			{
				return mvCloseCommand;
			}
		}

		public RelayCommand SetMergeCommand
		{
			get
			{
				return mvSetMergeCommand;
			}
		}
		
		#endregion


		#region Methods

		private void LoadData()
		{
			IsLoading = true;
			IsEnabled = false;

			if (Tables == null)
			{
				Tables = new ObservableCollection<TableViewModel>();
			}

			Tables.Clear();

			IEnumerable<TableViewModel> tables = null;

			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				tables = Engine.Instance.LoadTables().Select(p => new TableViewModel(p));

				Application.Current.Dispatcher.Invoke(() =>
				{
					foreach (var item in tables)
					{
						Tables.Add(item);
					}

					IsEnabled = true;
					IsLoading = false;
					
					RaiseRefresh();

				}, DispatcherPriority.Normal);
			}));
		}


		private void LoadTable(TableViewModel target)
		{

			var country = Tables.FirstOrDefault(p => p.Id == target.Id);
			if (country != null)
			{
				SelectedTable = country;
				//eventAggregator.GetEvent<ComboChangeEvent>().Publish(SelectedCountry);
			}
		}

		private void OnDataChanged(object obj)
		{
			//EmployesItemsViewModel vm = obj as EmployesItemsViewModel;
			//if (vm != null && vm == this)
			//{
			//	this.RaisePropertyChanged(() => IsHasError);
			//	RefreshCommands();
			//}
		}

		private void OnSelectedChanged(TableViewModel obj)
		{
			RefreshCommands();
		}

		//private bool Filter(object obj)
		//{
		//	EmployeeViewModel empl = obj as EmployeeViewModel;
		//	return empl != null && Employers.Contains(empl);
		//}

		private void RefreshCommands()
		{
			RefreshCommand.RaiseCanExecuteChanged();
			SaveCommand.RaiseCanExecuteChanged();
			EditCommand.RaiseCanExecuteChanged();
			DeleteCommand.RaiseCanExecuteChanged();
			AddCommand.RaiseCanExecuteChanged();
			CloseCommand.RaiseCanExecuteChanged();
			SettingShowCommand.RaiseCanExecuteChanged();
			SetMergeCommand.RaiseCanExecuteChanged();
		}

		private bool CanRefresh()
		{
			return IsEnabled && State == enFormState.None;
		}

		private bool CanSave()
		{
			return IsEnabled && !IsHasError && IsSelected && State != enFormState.None;
		}

		private bool CanShowSettings()
		{
			return IsSelected && IsCreate;
		}

		private bool CanEdit()
		{
			return IsEnabled && IsSelected;
		}

		private bool CanDelete()
		{
			return IsEnabled && IsSelected && State == enFormState.None;
		}

		private bool CanAdd()
		{
			return IsEnabled && State != enFormState.Create;
		}

		private bool CanMerge()
		{
			return IsEnabled && IsSelected && State != enFormState.Create;
		}

		private void OnClose()
		{
			//var mess = CEventSystem.Current.GetEvent<Events.CloseWindowEvent>();
			MessengerInstance.Send<CloseWindowMessage>(new CloseWindowMessage(), Token);
		}

		public void OnStateChanged()
		{
			RaiseRefresh();
		}

		private void OnRefresh()
		{
			SelectedTable = null;
			RaiseRefresh();

			LoadData();

			RaiseRefresh();
		}

		private void RaiseRefresh()
		{
			RaisePropertyesChanged();
			RefreshCommands();
		}

		private void OnSaveSelected()
		{
			if (SelectedTable.Current.Status == Status.Normal)
			{
				SelectedTable.Current.Status = Status.Updated;
			}

			ChangeState(true);

			//SelectedTable.Current.CountryID = SelectedCountry.Current.Id;
			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				SelectedTable.Current.Save();

				Application.Current.Dispatcher.Invoke(() =>
				{
					SelectedTable = null;

					//RaiseRefresh();

				}, DispatcherPriority.Normal);

				ChangeState(false);
				State = enFormState.None;
			}));

		}

		private void OnDeleteSelected()
		{
			SelectedTable.Current.Status = Status.Deleted;
			SelectedTable.Current.Save();

			this.Tables.Remove(SelectedTable);
			SelectedTable = null;

			RaiseRefresh();
		}
		
		private void OnAdd()
		{
			IsEnabled = false;

			CParserGenericAdapter proccesor = new CParserGenericAdapter();
			string path = CFileHelper.GetPathFromDialog();

			if (string.IsNullOrWhiteSpace(path))
			{
				IsEnabled = true;
				return;
			}

			IsLoading = true;
			State = enFormState.Create;

			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				try
				{
					DataTable data = proccesor.GetDataTabletFromCSVFile(path);
					CTable table = CTable.Create(data);

					BeginInvoke(DispatcherPriority.Normal, () =>
					{
						SelectedTable = new TableViewModel(table);

						SelectedTable.Current.Status = Status.Added;
						
						this.Tables.Add(SelectedTable);
						RaiseRefresh();
						this.RaisePropertyChanged(() => this.SelectedTableView);
					});
				}
				catch (Exception ex)
				{ }

				ChangeState(false);
			}));

		}

		public bool IsAppenedSuccessfully
		{
			get
			{
				return mvIsAppenedSuccessfully;
			}
			set
			{
				if (value == mvIsAppenedSuccessfully)
					return;

				mvIsAppenedSuccessfully = value;

				RaisePropertyChanged(() => this.IsAppenedSuccessfully);
			}
		}

		private void SetMerge()
		{
			IsEnabled = false;

			CParserGenericAdapter proccesor = new CParserGenericAdapter();
			string path = CFileHelper.GetPathFromDialog();

			if (string.IsNullOrWhiteSpace(path))
			{
				IsEnabled = true;
				return;
			}

			IsLoading = true;
			State = enFormState.EditTable;

			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				try
				{
					DataTable data = proccesor.GetDataTabletFromCSVFile(path);
					bool res = CTable.FillColumns(data, SelectedTable.Current, true);
					res &= CTable.FillRows(data, SelectedTable.Current);

					BeginInvoke(DispatcherPriority.Normal, () =>
					{
						IsAppenedSuccessfully = res;
						RaiseRefresh();
						this.RaisePropertyChanged(() => this.SelectedTableView);
					});
				}
				catch (Exception ex)
				{ }

				ChangeState(false);
			}));

		}


		private void OnEdit()
		{
			CWindowHelper.ShowEditSelectedTable(SelectedTable.Current);
		}

		private void SettingShow()
		{
			ChangeState(true);

			CWindowHelper.ShowEmployeWindow(SelectedTable.Current, null, OnSettingsTypesClosed);
		}

		private void OnSettingsTypesClosed()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				try
				{
					SelectedTable.Current.Rows.Clear();
					SelectedTable.Current.Columns.Clear();
					CTable.FillColumns(SelectedTable.Current.Data, SelectedTable.Current);
					CTable.FillRows(SelectedTable.Current.Data, SelectedTable.Current);
				}
				catch (Exception ex)
				{ }

				ChangeState(false);
			}));
		}

		private void ChangeState(bool isLoading)
		{
			BeginInvoke(DispatcherPriority.Normal, () =>
			{
				IsEnabled = !isLoading;
				IsLoading = isLoading;
				RaiseRefresh();
			});
		}

		#endregion
	}
}
