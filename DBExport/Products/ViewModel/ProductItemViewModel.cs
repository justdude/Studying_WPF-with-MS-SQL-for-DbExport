using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Data;
using DBExport.Common.MVVM;

namespace DBExport.Products.ViewModel
{
	public class ProductItemViewModel : ViewModelExtended
	{
		public List<CValue> RowItems { get; set; }

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
