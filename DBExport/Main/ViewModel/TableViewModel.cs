using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DbExport.Data;
using Microsoft.Practices.Prism.Mvvm;

namespace DBExport.Main.ViewModel
{
	public class TableViewModel : BindableBase
	{

		public TableViewModel(CTable table)
		{
			this.Current = table;
		}

		public CTable Current { get; private set; }

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

				this.OnPropertyChanged(() => this.Name);
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
