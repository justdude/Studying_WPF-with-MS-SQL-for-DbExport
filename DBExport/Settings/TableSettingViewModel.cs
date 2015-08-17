using DBExport.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
//using Microsoft.Practices.Prism.Commands;
//using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Settings.ViewModel
{
	public class TableSettingViewModel : ViewModelBase
	{
		private readonly RelayCommand mvSaveCommand;
		private readonly RelayCommand mvCloseCommand;
		private Dictionary<string, Type> selectedTypes;

		public TableSettingViewModel(Dictionary<string, Type> selectedTypes)
		{
			this.selectedTypes = selectedTypes;

			Tables = new ObservableCollection<TableSettingItemViewModel>();
			DataTypes = new List<Type>()
			{
				typeof(string),
				typeof(float),
				typeof(int),
				typeof(DateTime),
				typeof(bool)
			};

			mvSaveCommand = new RelayCommand(OnSaveSelected, CanSave);
			mvCloseCommand = new RelayCommand(OnClose);

			foreach (var item in Tables)
			{
				item.CurrentType = DataTypes[0];
			}

			//eventAggregator.GetEvent<StateChangedEvent>().Subscribe(OnDataChanged);
			//eventAggregator.GetEvent<ItemChangedEvent>().Subscribe(OnSelectedChanged, ThreadOption.PublisherThread, true, Filter);
		}

		public ObservableCollection<TableSettingItemViewModel> Tables { get; set; }
		public List<Type> DataTypes { get; set; }

		private bool CanSave()
		{
			return Tables.Any(p => p.IsHasErrors) == false;
		}

		private void OnSaveSelected()
		{
			selectedTypes.Clear();
			try
			{
				foreach (var item in Tables)
				{
					selectedTypes.Add(item.Name, item.CurrentType);
				}
			}
			catch(Exception)
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
