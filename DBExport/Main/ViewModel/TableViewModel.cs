using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DbExport.Data;
//using Microsoft.Practices.Prism.Mvvm;
using GalaSoft.MvvmLight;

namespace DBExport.Main.ViewModel
{
	public class TableViewModel : ViewModelBase
	{

		private bool? mvIsAppenedSuccessfully = null;

		public TableViewModel(CTable table)
		{
			this.Current = table;
		}

		public CTable Current { get; private set; }

		public enFormState State { get; set; }

		public bool? IsAppenedSuccessfully
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

		public bool IsExist
		{
			get
			{
				return Current != null;
			}
		}

		public string Name
		{
			get
			{
				return IsExist ? Current.Name : string.Empty;
			}
			set
			{
				Current.Name = value;

				this.RaisePropertyChanged(() => this.Name);
			}
		}

		public string Id
		{
			get
			{
				return IsExist ? Current.Id : string.Empty;
			}
		}

		public void RaisePropertyesChanged()
		{
		}
	}
}
