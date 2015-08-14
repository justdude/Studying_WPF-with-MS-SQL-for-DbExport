using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Interfaces;
using GenericParsing;

namespace DbExport.CSV
{
	public class CParserGenericAdapter : IDataParser
	{
		#region Члены IDataParser

		public System.Data.DataSet Parse(string path)
		{
			try
			{
				DataSet dsResult;

				// Using an XML Config file. 
				using (GenericParserAdapter parser = new GenericParserAdapter(path))
				{
					parser.Load(path);

					parser.SetDataSource("MyData.txt");

					//parser.ColumnDelimiter = "\t".ToCharArray();
					parser.ColumnDelimiter = '|';
					parser.FirstRowHasHeader = true;
					parser.SkipStartingDataRows = 10;
					parser.MaxBufferSize = 4096;
					parser.MaxRows = 500;
					parser.TextQualifier = '|';

					dsResult = parser.GetDataSet();
				}
			}
			catch (Exception ex)
			{ 
				
			}
		}

		#endregion
	}
	}
}
