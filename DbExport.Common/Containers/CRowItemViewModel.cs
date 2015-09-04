using DbExport.Common.Interfaces;
using DBExport.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Common.Containers
{
	public class CRowItemViewModel : IDataValue, IDataErrorInfo, INotifyPropertyChanged
	{
		public IObjectBase Coll { get; set; }
		public IDataValue Value { get; set; }
		public Func<string, IDataValue, IObjectBase, string> OnValidateErrors { get; set; }
		public Action<string, string, string> OnPropertyChanged { get; set; }

		public CRowItemViewModel(IDataValue val)
		{
			Value = val;
		}

		#region INotifyPropertyChanged

		public void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private string modErrorText;

		#endregion

		#region IDataValue

		public string CollumnName
		{
			get
			{
				return Value.CollumnName;
			}
		}

		public Type ValueType
		{
			get
			{
				return Value.ValueType;
			}
			set
			{
				if (value == Value.ValueType)
					return;

				Value.ValueType = value;
			}
		}

		public DateTime DateValue
		{
			get
			{
				return Value.DateValue;
			}
			set
			{
				if (value == Value.DateValue)
					return;

				Value.DateValue = value;

				RaisePropertyChanged("DateValue");
				RaiseOnPropertyChanged("DateValue"); 
			}
		}

		public string StrValue
		{
			get
			{
				return Value.StrValue;
			}
			set
			{
				if (value == Value.StrValue)
					return;

				Value.StrValue = value;

				RaisePropertyChanged("StrValue");
				RaiseOnPropertyChanged("StrValue"); 
			}
		}

		public bool BoolValue
		{
			get
			{
				return Value.BoolValue;
			}
			set
			{
				if (value == Value.BoolValue)
					return;

				Value.BoolValue = value;

				RaisePropertyChanged("BoolValue");
				RaiseOnPropertyChanged("BoolValue"); 
			}
		}

		public double FloatValue
		{
			get
			{
				return Value.FloatValue;
			}
			set
			{
				if (value == Value.FloatValue)
					return;

				Value.FloatValue = value;

				RaisePropertyChanged("FloatValue");
				RaiseOnPropertyChanged("FloatValue"); 
			}
		}

		public int IntValue
		{
			get
			{
				return Value.IntValue;
			}
			set
			{
				if (value == Value.IntValue)
					return;

				Value.IntValue = value;

				RaisePropertyChanged("IntValue");
				RaiseOnPropertyChanged("IntValue"); 
			}
		}

		public void SetValue(object data)
		{
			Value.SetValue(data);
		}

		public object GetValue()
		{
			return Value.GetValue();
		}

		#endregion

		#region IDataErrorInfo

		public string Error
		{
			get { throw new NotImplementedException(); }
		}

		public string this[string columnName]
		{
			get 
			{
				modErrorText = ValidateError(columnName, Value, Coll);
				return modErrorText;
			}
		}

		private string ValidateError(string columnName, IDataValue value_, IObjectBase coll)
		{
			if (OnValidateErrors == null)
				return string.Empty;

			return OnValidateErrors(columnName, value_, coll);
		}

		#endregion


		public bool IsHasErrors
		{
			get
			{
				return !string.IsNullOrWhiteSpace(modErrorText);
			}
		}

		private void RaiseOnPropertyChanged(string name)
		{
			if (OnPropertyChanged == null)
				return;
			
			OnPropertyChanged(name, Value.CollumnName, Coll.Id);
		}

		//private bool mvIsHasErrors;

		//public bool IsHasErrors
		//{
		//	get { return mvIsHasErrors; }
		//	set {
		//		if (value == mvIsHasErrors)
		//			return;

		//		mvIsHasErrors = value;
		//		RaisePropertyChanged("IsHasErrors");
		//	}
		//}


	}
}
