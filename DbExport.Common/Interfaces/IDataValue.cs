using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Common.Interfaces
{
	public interface IDataValue
	{
		DateTime DateValue { get; set; }
		String StrValue { get; set; }
		bool BoolValue { get; set; }
		double FloatValue { get; set; }
		int IntValue { get; set; }

		Type ValueType { get; set; }

		string CollumnName { get; }

		void SetValue(object data);
		object GetValue();
	}
}
