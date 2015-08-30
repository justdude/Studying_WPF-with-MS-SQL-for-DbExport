using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Common.Validation
{
	public class NameRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			string str = value as string;

			return new ValidationResult(str.Length > 10, "Must be more than 10 symbols");
		}
	}
}
