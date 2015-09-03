using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Data;
using DBExport.Common.MVVM;
using DbExport.Controls.GeneratableForm;
using DBExport.Common.Containers;
using DbExport.Common.Interfaces;
using DBExport.Common.Interfaces;
using DBExport.Common.Messages;
using DBExport.Common.Constants;

namespace DBExport.Products.ViewModel
{
	public class ProductItemViewModel : ViewModelExtended
	{
		private List<CRowItemViewModel> mvRowitemsViewModels;
		private bool mvIsChanged;

		public List<CValue> RowItems { get; set; }

		public List<CRowItemViewModel> RowitemsViewModels
		{
			get
			{

				if (mvRowitemsViewModels == null)
				{
					if (RowItems == null)
					{
						mvRowitemsViewModels = null;
						return null;
					}

					InitRows();
				}

				return mvRowitemsViewModels;
			}
		}

		public bool IsHasErrors
		{
			get
			{
				return RowitemsViewModels.Any(p => p.IsHasErrors == true);
			}
		}

		private void InitRows()
		{
			CRowItemViewModel vm = null;
			mvRowitemsViewModels = new List<CRowItemViewModel>();

			foreach (CValue item in RowItems)
			{
				vm = new CRowItemViewModel(item as DBExport.Common.Interfaces.IDataValue);
				vm.Coll = item.Column;
				vm.OnValidateErrors = OnItemValidationErrors;
				vm.OnPropertyChanged = OnItemChanged;
				mvRowitemsViewModels.Add(vm);
			}
		}

		private void OnItemChanged(string propName)
		{
			CValue changedItem = RowItems.FirstOrDefault(p => p.CollumnName == propName);
			if (changedItem != null && changedItem.Status == Status.Normal)
			{
				changedItem.Status = Status.Updated;
			}

			IsChanged = true;
			MessengerInstance.Send<PropertyChangedMessage>(new PropertyChangedMessage() { PropName = propName }, Token);
		}

		private string OnItemValidationErrors(string columnName, IDataValue value, IObjectBase coll)
		{
			string errors = string.Empty;

			switch (columnName)
			{
				case TypeNames.BoolValue:
					break;
				case TypeNames.DateValue:
					break;
				case TypeNames.FloatValue:
					break;
				case TypeNames.StrValue:
					break;
				case TypeNames.IntValue:
					break;
				default:
					break;
			}


			MessengerInstance.Send<ErrorStateMessage>(new Common.Messages.ErrorStateMessage(), Token);

			return errors;
		}

		//public string Id
		//{
		//	get
		//	{ 
		//		if (RowItem == null)
		//			return string.Empty;

		//		return RowItem.Id;
		//	}
		//}

		public string Name
		{
			get
			{
				return DbExport.Data.Engine.Instance.GetName(RowItems);
			}
		}

		public bool IsChanged
		{
			get
			{
				return mvIsChanged;
			}
			set
			{
				if (mvIsChanged == value)
					return;

				mvIsChanged = value;

				RaisePropertyChanged(() => this.IsChanged);
			}
		}

		public void RaiseRefresh()
		{ 
			
		}

		public void RaisePropertyesChanged()
		{
			this.RaisePropertyChanged(() => this.Name);
		}

		public void RefreshCommands()
		{ 
		
		}
	}
}
