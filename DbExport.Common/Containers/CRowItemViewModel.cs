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
	public class CRowItemViewModel : IDataValue, INotifyPropertyChanged
	{
		public IObjectBase Coll { get; set; }
		public IDataValue Value { get; set; }

		public CRowItemViewModel(IDataValue val)
		{
			Value = val;
		}

		#region INotifyPropertyChanged

		public void RaiseProprtyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

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
				Value.DateValue = value;
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
				Value.StrValue = value;
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
				Value.BoolValue = value;
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
				Value.FloatValue = value;
			}
		}

		public float IntValue
		{
			get
			{
				return Value.IntValue;
			}
			set
			{
				Value.IntValue = value;
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
	}
}
