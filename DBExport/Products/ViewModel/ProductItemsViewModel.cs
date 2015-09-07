using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DbExport.Common.Interfaces;
using DbExport.CSV;
using DbExport.Data;
using DbExport.Database;
using DBExport.Common.Containers;
using DBExport.Common.Messages;
using DBExport.Common.MVVM;
using DBExport.Helpers;
using DBExport.Main;
using DBExport.Main.ViewModel;
using DBExport.Products.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DBExport.Products
{
	public class ProductItemsViewModel : ListViewModelExtended<ProductItemViewModel>
	{
		//private ProductItemViewModel mvSelectedTable;

		private readonly RelayCommand mvAddCommand;
		private readonly RelayCommand mvFilterCommand;
		private readonly RelayCommand mvDeleteCommand;
		private readonly RelayCommand mvSaveCommand;
		private readonly RelayCommand mvRefreshCommand;
		private readonly RelayCommand mvCloseCommand;

		private bool mvIsBlocked;
		private bool mvIsHasError;
		private bool mvIsMerge;
		private bool mvIsLoading;
		private ProductItemViewModel mvSelectedRowsItem;
		private bool mvIsChanged;
		private List<CCollumnItem> modColumns;
		private string mvSearchString;
		private List<CFilter> modFilterQueries;

		public ProductItemsViewModel(DbExport.Data.CTable table)
		{
			this.SelectedTable = table;

			mvAddCommand = new RelayCommand(OnAdd, CanAdd);
			mvFilterCommand = new RelayCommand(OnFilterCommand, CanFilter);
			mvDeleteCommand = new RelayCommand(OnDeleteSelected, CanDelete);
			mvSaveCommand = new RelayCommand(OnSaveSelected, CanSave);
			mvRefreshCommand = new RelayCommand(OnRefresh, CanRefresh);
			mvCloseCommand = new RelayCommand(OnClose);

			modFilterQueries = new List<CFilter>();
			RowItems = new ObservableCollection<ProductItemViewModel>();
			base.SelectedItems = new ObservableCollection<ProductItemViewModel>();

			OnDispatcherChanged += OnDispatcherChange;
		}

		private void OnDispatcherChange(System.Windows.Threading.Dispatcher obj)
		{
		}

		protected override void OnTokenChanged()
		{
			RegisterMessenger();
			LoadData();
			base.OnTokenChanged();
		}

		private void RegisterMessenger()
		{
			MessengerInstance.Register<PropertyChangedMessage>(this, Token, OnPropertiesChanged);
			MessengerInstance.Register<ErrorStateMessage>(this, Token, OnErrorStateChanged);
		}

		#region Properties

		public ProductItemViewModel SelectedRowsItem
		{
			get
			{
				return mvSelectedRowsItem;
			}
			set
			{
				if (mvSelectedRowsItem == value)
					return;

				mvSelectedRowsItem = value;

				OnSelectedChanged(value);

				//this.RaisePropertyChanged(() => this.SelectedTable);
				//this.RaisePropertyChanged(() => this.SelectedTableView);
				this.RaisePropertyChanged(() => this.SelectedRowsItem);
			}
		}

		public ObservableCollection<ProductItemViewModel> RowItems { get; set; }

		public CTable SelectedTable
		{
			get;
			private set;
		}

		public DataView SelectedTableView
		{
			get
			{
				if (!IsSelected)
					return null;

				return CDataTableHelper.GetItems(SelectedTable.Data, 0, 20).DefaultView;
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


		public string SearchString
		{
			get
			{
				return mvSearchString;
			}
			set
			{
				if (mvSearchString == value)
					return;

				mvSearchString = value;

				FilterByName(mvSearchString);
				RaiseRefresh();

				this.RaisePropertyChanged(() => this.SearchString);
			}
		}

		private void FilterByName(string searchString)
		{
			foreach (ProductItemViewModel item in RowItems)
			{

				if (item.Name.StartsWith(searchString))
				{
					item.IsVisible = true;
				}
				else
					item.IsVisible = false;
			}
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

		public bool IsChanged
		{
			get
			{
				return IsSelected && SelectedRowsItem.IsChanged;
			}
		}

		public bool IsHasError
		{
			get
			{
				return IsSelected && SelectedRowsItem.IsHasErrors;
			}
		}

		public bool IsSelected
		{
			get
			{
				return SelectedRowsItem != null && SelectedTable != null && SelectedTable.Data != null;
			}
		}

		public bool IsSelectedRightPart
		{
			get
			{
				return IsEnabled && SelectedRowsItem != null && SelectedTable != null && SelectedTable.Data != null;
			}
		}

		#endregion

		#region Члены IDataButtons

		public RelayCommand AddCommand
		{
			get
			{
				return mvAddCommand;
			}
		}

		public RelayCommand FilterCommand
		{
			get
			{
				return mvFilterCommand;
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

		#endregion


		#region Methods

		private void RaisePropertyesChanged()
		{
			//if (IsSelected)
			//	SelectedTable.RaisePropertyesChanged();

			this.RaisePropertyChanged(() => this.IsSelectedRightPart);
			this.RaisePropertyChanged(() => this.IsHasError);
			this.RaisePropertyChanged(() => this.IsSelected);
			this.RaisePropertyChanged(() => this.IsEnabled);
		}

		private void OnPropertiesChanged(PropertyChangedMessage obj)
		{
			State = enFormState.EditTable;

			RaisePropertyesChanged();
			RefreshCommands();
		}

		private void OnErrorStateChanged(ErrorStateMessage obj)
		{
			RaisePropertyesChanged();
			RefreshCommands();
		}

		private void LoadSelectedRow(ProductItemViewModel item)
		{
			try
			{
				List<CRowItemViewModel> rowList = null;

				if (item == null)
				{
					rowList = new List<CRowItemViewModel>();
				}
				else
				{
					rowList = item.RowitemsViewModels;
				}


				MessengerInstance.Send<Common.Messages.LoadRowsMessage>(new LoadRowsMessage() { Rows = rowList }, Token);
			}
			catch (Exception ex)
			{ }
		}

		private void LoadData()
		{
			IsLoading = true;
			IsEnabled = false;

			//ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			//{

			if (!IsCollumnsLoaded)
			{
				modColumns = SelectedTable.Columns.Select(p => new CCollumnItem()
				{
					ItemType = p.TargetType,
					Name = p.Name,
					Coll = p
				}).ToList();

				MessengerInstance.Send<Common.Messages.LoadCollumnsMessage>(new LoadCollumnsMessage() { Collumns = modColumns }, Token);
				IsCollumnsLoaded = true;
			}

			List<string> hidedId = new List<string>();
			ManualResetEvent[] resEvent = new ManualResetEvent[] { new ManualResetEvent(false), new ManualResetEvent(false) };
			IEnumerable<List<CValue>> dataRowsList = null;

			BeginInvoke(DispatcherPriority.Background, () =>
				{
					LoadFilters(hidedId);

					dataRowsList = SelectedTable.Rows.Where(p => !hidedId.Contains(p.Id))
													.GroupBy(p => p.RowNumb)
													.Select(p => p.ToList());
													 RowItems.Clear();

					 BeginInvoke(DispatcherPriority.Background, () =>
						 {
							 ProductItemViewModel pr = null;
							 foreach (var items in dataRowsList)
							 {
								 pr = new ProductItemViewModel();
								 pr.RowItems = items;
								 pr.Token = Token;

								 RowItems.Add(pr);
								 //pr.RaisePropertyesChanged();
							 }

							 Application.Current.Dispatcher.Invoke(() =>
							 {
								 IsEnabled = true;
								 IsLoading = false;

								 RaiseRefresh();

							 }, DispatcherPriority.Normal);
						 });

				});
		
			//}));
		}

		private void LoadFilters(List<string> hidedId)
		{
				try
				{
					var tr = CDatabase.Instance.BeginTransaction();

					foreach (CFilter item in modFilterQueries)
					{
						List<string> ids = item.ExecuteQuery(tr);
						hidedId.AddRange(ids);
					}
				}
				catch (Exception ex)
				{ }
		}

		private void OnDataChanged(object obj)
		{
			ProductItemViewModel vm = obj as ProductItemViewModel;
			if (vm != null)
			{
				this.RaisePropertyChanged(() => IsHasError);
				RefreshCommands();
			}
		}

		private void OnItemChanged(DataRowView rowView, IObjectBase data, Status status)
		{
			if (rowView == null)
				return;

			rowView.Delete();
			data.Status = Status.Deleted;
		}

		private void OnSelectedChanged(ProductItemViewModel obj)
		{
			LoadSelectedRow(obj);
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
			FilterCommand.RaiseCanExecuteChanged();
			DeleteCommand.RaiseCanExecuteChanged();
			AddCommand.RaiseCanExecuteChanged();
			CloseCommand.RaiseCanExecuteChanged();
		}

		private bool CanRefresh()
		{
			return IsEnabled && !IsSelected || (IsSelected && State == enFormState.None);
		}

		private bool CanSave()
		{
			return IsEnabled && IsSelected && !IsHasError && IsChanged;//&& State != enFormState.None;
		}

		private bool CanFilter()
		{
			return IsEnabled && State == enFormState.None;
		}

		private bool CanDelete()
		{
			return IsEnabled && IsSelected && State == enFormState.None;
		}

		private bool CanAdd()
		{
			return IsEnabled && !IsSelected || (IsSelected && State == enFormState.None);
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
			SelectedRowsItem = null;

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
			ChangeState(true);

			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				if (State == enFormState.Create)
				{
					SelectedTable.Rows.AddRange(SelectedRowsItem.RowItems);
					SelectedTable.Save();
				}

				if (State == enFormState.None || State == enFormState.EditTable)
				{
					foreach (ProductItemViewModel item in SelectedItems)
					{
						if (item == SelectedRowsItem && SelectedItems.Count > 1)
							continue;

						item.RowItems.ForEach(p => CopyDataTo(p, SelectedRowsItem.RowItems));
						Invoke(DispatcherPriority.Normal, () => item.RaisePropertyesChanged());
					}

					SelectedTable.Save();
				}

				Application.Current.Dispatcher.Invoke(() =>
				{
					SelectedRowsItem.IsChanged = false;
					SelectedRowsItem = null;
					SelectedItems.Clear();
					State = enFormState.None;
					RaiseRefresh();

				}, DispatcherPriority.Normal);

				ChangeState(false);

			}));

		}

		private object CopyDataTo(CValue target, List<CValue> list)
		{
			CValue source = list.FirstOrDefault(p => p.CollumnId == target.CollumnId);
			if (source == null)
				return null;

			target.SetValue(source.GetValue());
			target.Status = source.Status;
			return target;
		}

		private void OnDeleteSelected()
		{
			IsEnabled = false;
			IsLoading = true;
			RaiseRefresh();

			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				DeleteSelected();
			}));

		}

		private void DeleteSelected()
		{
			try
			{
				var tr = CDatabase.Instance.BeginTransaction();

				bool res = true;
				var selectionCopy = SelectedItems.ToList();
				foreach (ProductItemViewModel item in selectionCopy)
				{
					item.RowItems.ForEach(p => p.Status = Status.Deleted);
					res = item.RowItems.SaveList(Status.Deleted, tr);
					SelectedTable.Rows.RemoveAll(p => ClearTable(item));
					if (res)
					{
						BeginInvoke(DispatcherPriority.Normal, () =>
						{
							RowItems.Remove(item);
						});
					}
				}

				BeginInvoke(DispatcherPriority.Normal, () =>
				{
					SelectedItems.Clear();
					SelectedRowsItem = null;
					RaiseRefresh();
				});
			}
			catch (Exception ex)
			{ }

			ChangeState(false);
		}

		private static bool ClearTable(ProductItemViewModel item)
		{
			var target = item.RowItems.FirstOrDefault();
			return target != null && target.Status == Status.Deleted;
		}

		private void OnAdd()
		{
			SelectedItems.Clear();
			SelectedRowsItem = null;
			RaiseRefresh();

			IsEnabled = false;
			IsLoading = false;

			var newListItem = new ProductItemViewModel();
			newListItem.Token = Token;

			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				try
				{
					var addedRows = CTable.CreateEmptyRows(SelectedTable).ToList();
					newListItem.RowItems = addedRows;

					BeginInvoke(DispatcherPriority.Normal, () =>
					{
						SelectedRowsItem = newListItem;
						State = enFormState.Create;
						RaiseRefresh();
					});
				}
				catch (Exception ex)
				{ }

				ChangeState(false);
			}));

		}

		private void OnFilterCommand()
		{
			CWindowHelper.ShowSelectFilterWindow(SelectedTable.Id, OnFilterSelected);
		}

		private void OnFilterSelected(string tableId, List<CFilter> newFilters)
		{
			modFilterQueries.Clear();
			modFilterQueries.AddRange(newFilters);
			LoadData();
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

		public enFormState State { get; set; }

		public bool IsCollumnsLoaded { get; set; }
	}
}
