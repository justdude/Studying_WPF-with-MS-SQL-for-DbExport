using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Data;
using DBExport.Common.MVVM;
using DbExport.Controls.GeneratableForm;
using DBExport.Common.Containers;

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
				mvRowitemsViewModels.Add(vm);
			}
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
