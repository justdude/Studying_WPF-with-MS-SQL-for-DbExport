using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DbExport.Interfaces;
using Microsoft.VisualBasic.FileIO;

namespace DbExport.CSV
{
	public class CParserGenericAdapter //: IDataParser
	{
		#region Члены IDataParser

		public DataTable GetDataTabletFromCSVFile(string csv_file_path)
		{
			DataTable csvData = new DataTable();
			try
			{
				csvData.TableName = Path.GetFileNameWithoutExtension(csv_file_path);
				using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
				{
					csvReader.SetDelimiters(new string[] { "|" });
					csvReader.HasFieldsEnclosedInQuotes = false;
					csvReader.TrimWhiteSpace = false;
					string[] colFields = csvReader.ReadFields();
					foreach (string column in colFields)
					{
						DataColumn datecolumn = new DataColumn(column);
						datecolumn.AllowDBNull = true;
						csvData.Columns.Add(datecolumn);
					}
					while (!csvReader.EndOfData)
					{
						string[] fieldData = csvReader.ReadFields();
						//Making empty value as null
						for (int i = 0; i < fieldData.Length; i++)
						{
							if (fieldData[i] == "")
							{
								fieldData[i] = null;
							}
						}
						csvData.Rows.Add(fieldData);
					}
				}
			}
			catch (Exception ex)
			{
			}
			return csvData;
		}

		#endregion
	}
}

