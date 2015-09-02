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

		private void InitRows()
		{
			CRowItemViewModel vm = null;
			mvRowitemsViewModels = new List<CRowItemViewModel>();

			foreach (CValue item in RowItems)
			{
				vm = new CRowItemViewModel(item as DBExport.Common.Interfaces.IDataValue);
				vm.Coll = item.Column;
				vm.OnValidateErrors = OnItemValidationErrors;
				mvRowitemsViewModels.Add(vm);
			}
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
				//case TypeCode.Int32:
				//	break;
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
