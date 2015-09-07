using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Translates
{
	public static class CTranslates
	{
		public const string HelpToFilterItems = "Select your query and after press refresh";
		public const string WaitingForStatesChanged = "You can edit table or append data";
		public const string AddingNewTable = "Press setting to select table types and save";
		public const string AddingAdditionalData = "Press Setting to select table types and Save to commit changes";
		public const string TableIsNotSelected = "Select table or create new";

		public static string IsHasErrors { get; set; }
	}
}
