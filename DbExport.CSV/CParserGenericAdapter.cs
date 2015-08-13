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
	public class CParserGenericAdapter//: IDataParser
	{
		#region Члены IDataParser

	//	public System.Data.DataSet Parse(string path)
	//	{

	//		try
	//		{
	//			DataSet dsResult;

	//			// Using an XML Config file. 
	//			using (GenericParserAdapter parser = new GenericParserAdapter(path))
	//			{
	//				parser.Load(path);
	//				dsResult = parser.GetDataSet();

	//				parser.SetDataSource("MyData.txt");

	//				parser.ColumnDelimiter = "\t".ToCharArray();
	//				parser.FirstRowHasHeader = true;
	//				parser.SkipStartingDataRows = 10;
	//				parser.MaxBufferSize = 4096;
	//				parser.MaxRows = 500;
	//				parser.TextQualifier = '\"';


	//			// Or... programmatically setting up the parser for Fixed-width. 
	//			using (GenericParser parser = new GenericParser())
	//			{
	//				parser.SetDataSource("MyData.txt");

	//				parser.ColumnWidths = new int[4] { 10, 10, 10, 10 };
	//				parser.SkipStartingDataRows = 10;
	//				parser.MaxRows = 500;

	//				while (parser.Read())
	//				{
	//					strID = parser["ID"];
	//					strName = parser["Name"];
	//					strStatus = parser["Status"];

	//					// Your code here ...
	//				}
	//			}
	//		}
	//		catch (Exception ex)
	//		{ 
				
	//		}
	//	}

	//	#endregion
	//}
}
