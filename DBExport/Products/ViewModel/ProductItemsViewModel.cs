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
using DBExport.Main;
using DBExport.Main.ViewModel;
using DBExport.Products.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DBExport.Products
{
	public class ProductItemsViewModel : ViewModelExtended
	{
		//private ProductItemViewModel mvSelectedTable;

		private readonly RelayCommand mvAddCommand;
		private readonly RelayCommand mvEditCommand;
		private readonly RelayCommand mvDeleteCommand;
		private readonly RelayCommand mvSaveCommand;
		private readonly RelayCommand mvRefreshCommand;
		private readonly RelayCommand mvCloseCommand;

		private bool mvIsBlocked;
		private bool mvIsHasError;
		private bool mvIsMerge;
		private bool mvIsLoading;
		private ProductItemViewModel mvSelectedRowsItem;

		public ProductItemsViewModel(DbExport.Data.CTable table)
		{
			this.SelectedTable = table;

			mvAddCommand = new RelayCommand(OnAdd, CanAdd);
			mvEditCommand = new RelayCommand(OnEdit, CanEdit);
			mvDeleteCommand = new RelayCommand(OnDeleteSelected, CanDelete);
			mvSaveCommand = new RelayCommand(OnSaveSelected, CanSave);
			mvRefreshCommand = new RelayCommand(OnRefresh, CanRefresh);
			mvCloseCommand = new RelayCommand(OnClose);

			RowItems = new ObservableCollection<ProductItemViewModel>();

			OnDispatcherChanged += OnDispatcherChange;
		}

		private void OnDispatcherChange(System.Windows.Threading.Dispatcher obj)
		{
		}

		protected override void OnTokenChanged()
		{
			LoadData();
			base.OnTokenChanged();
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

				this.RaisePropertyChanged(() => this.SelectedTable);
				this.RaisePropertyChanged(() => this.SelectedTableView);
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
				return SelectedTable != null && SelectedTable.Data != null;
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

			this.RaisePropertyChanged(() => this.IsHasError);
			this.RaisePropertyChanged(() => this.IsSelected);
			this.RaisePropertyChanged(() => this.IsEnabled);
		}



		private void LoadSelectedRow(ProductItemViewModel item)
		{
			try
			{
				if (item == null)
					return;

				List<CRowItem> items = item.RowItems.Select(p =>
					new CRowItem()
					{
						Name = p.Column.Name,
						Value = p.GetValue(),
						ValueType = p.ValueType
					}).ToList();

				MessengerInstance.Send<Common.Messages.LoadRowsMessage>(new LoadRowsMessage() { Rows = items }, Token);
			}
			catch(Exception ex)
			{}
		}

		private void LoadData()
		{
			IsLoading = true;
			IsEnabled = false;

			//ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			//{
			IEnumerable<List<CValue>> dataRowsList = SelectedTable.Rows.GroupBy(p => p.RowNumb).Select(p => p.ToList());

			RowItems.Clear();

			foreach (var items in dataRowsList)
			{
				ProductItemViewModel pr = new ProductItemViewModel();
				pr.RowItems = items;

				RowItems.Add(pr);

				pr.RaisePropertyesChanged();
				//Application.Current.Dispatcher.Invoke(() => pr.RaisePropertyesChanged(), DispatcherPriority.Normal);
			}

			if (!IsCollumnsLoaded)
			{
				var items = SelectedTable.Columns.Select(p => new CCollumnItem() { ItemType = p.TargetType, Name = p.Name }).ToList();

				MessengerInstance.Send<Common.Messages.LoadCollumnsMessage>(new LoadCollumnsMessage() { Collumns = items }, Token);
				IsCollumnsLoaded = true;
			}

			Application.Current.Dispatcher.Invoke(() =>
			{
				IsEnabled = true;
				IsLoading = false;

				RaiseRefresh();

			}, DispatcherPriority.Normal);
			//}));
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
			EditCommand.RaiseCanExecuteChanged();
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
			return IsEnabled && !IsHasError && IsSelected && State != enFormState.None;
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
			return IsEnabled && !IsSelected || (IsSelected && State != enFormState.Create);
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
			if (SelectedTable.Status == Status.Normal)
			{
				SelectedTable.Status = Status.Updated;
			}

			ChangeState(true);

			//SelectedTable.Current.CountryID = SelectedCountry.Current.Id;
			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				SelectedTable.Save();

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
			SelectedTable.Status = Status.Deleted;
			SelectedTable.Save();

			//this.Tables.Remove(SelectedTable);
			SelectedTable = null;

			RaiseRefresh();
		}

		private void OnAdd()
		{
			IsEnabled = false;
			IsLoading = true;

			ThreadPool.QueueUserWorkItem(new WaitCallback((par) =>
			{
				try
				{
					//data.Rows.Add(new object[] { });

					BeginInvoke(DispatcherPriority.Normal, () =>
					{
						SelectedTable.Status = Status.Added;

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
