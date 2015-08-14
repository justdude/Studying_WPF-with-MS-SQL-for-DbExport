using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CRM.Common.Validation
{
	public class DateRule : ValidationRule
	{
		private static DateTime CurrentDate = new DateTime(1985, 1, 1);

		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			string errorContent = null;

			if (value == null)
			{
				errorContent = "Empty datetime";
				return new ValidationResult(false, errorContent);
			}
				

			DateTime orderDate = (DateTime)value;

			if (orderDate >= DateTime.Now)
			{
				errorContent = "Please, enter date before Now()";
				return new ValidationResult(false, errorContent);
			}

			bool res = orderDate < CurrentDate;
			errorContent = "No less than 01/01/1985";

			if (!res)
			{
				errorContent = null;
			}

			return new ValidationResult(!res, errorContent);
		}

		public static bool IsValid(object value, out string errorContent)
		{
			errorContent = null;

			if (value == null)
			{
				errorContent = "Empty datetime";
				return false;
			}


			DateTime orderDate = (DateTime)value;

			if (orderDate >= DateTime.Now)
			{
				errorContent = "Please, enter date before Now()";
				return false;
			}

			bool res = orderDate < CurrentDate;
			errorContent = "No less than 01/01/1985";

			if (!res)
			{
				errorContent = null;
			}

			return !res;
		}
	}
}
