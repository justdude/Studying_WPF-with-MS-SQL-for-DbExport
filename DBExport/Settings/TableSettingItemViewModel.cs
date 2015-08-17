using GalaSoft.MvvmLight;
//using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Settings.ViewModel
{
	public class TableSettingItemViewModel : ViewModelBase
	{
		private bool mvIsHasErrors;
		private Type mvCurrentType;

		public string Name
		{
			get
			{
				return CurrentType.Name;
			}
		}

		public Type CurrentType
		{
			get
			{
				return mvCurrentType;
			}
			set
			{
				if (value == mvCurrentType)
					return;

				mvCurrentType = value;

				this.RaisePropertyChanged(() => this.CurrentType);
			}
		}

		public bool IsHasErrors
		{ 
			get
			{
				return mvIsHasErrors;
			}
			set
			{
				if (value == mvIsHasErrors)
					return;

				mvIsHasErrors = value;

				this.RaisePropertyChanged(() => this.IsHasErrors);
			}
		}

	}
}
