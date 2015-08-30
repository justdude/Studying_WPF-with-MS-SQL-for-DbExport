using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Database
{
	public class CDataTableHelper
	{
		public static DataTable ConvertTableType(DataTable dt, Dictionary<string, Type> types)
		{
			DataTable newDt = dt.Clone();
			//convert all columns' datatype

			newDt.TableName = dt.TableName;

			foreach (DataColumn dc in newDt.Columns)
			{
				if (types.ContainsKey(dc.ColumnName))
				{
					dc.DataType = types[dc.ColumnName];
				}
			}

			//import data from original table
			//foreach (DataRow dr in dt.Rows)
			//{
			//	newDt.ImportRow(dr);
			//}

			FillData(dt, newDt);

			dt.Dispose();
			return newDt;
		}

		public static void FillData(DataTable dt, DataTable newDt)
		{
			for (int row = 0; row < dt.Rows.Count; row++)
			{
				object[] newRowValues = new object[newDt.Columns.Count];
				try
				{
					for (int coll = 0; coll < newDt.Columns.Count; coll++)
					{
						object item = dt.Rows[row].ItemArray[coll];

						if (newDt.Columns[coll].DataType == typeof(float))
						{
							float flRes = 0;
							if (float.TryParse(item.ToString(), out flRes))
							{
								newRowValues[coll] = flRes;
							}
						}

						if (newDt.Columns[coll].DataType == typeof(int))
						{
							int intRes = 0;
							if (int.TryParse(item.ToString(), out intRes))
							{
								newRowValues[coll] = intRes;
							}
						}

						if (newDt.Columns[coll].DataType == typeof(DateTime))
						{
							DateTime dateRes = DateTime.MinValue;
							if (DateTime.TryParse(item.ToString(), out dateRes))
							{
								newRowValues[coll] = dateRes;
							}
						}

						if (newDt.Columns[coll].DataType == typeof(bool))
						{
							bool boolRes = false;
							if (bool.TryParse(item.ToString(), out boolRes))
							{
								newRowValues[coll] = boolRes;
							}
						}

						if (newDt.Columns[coll].DataType == typeof(string))
						{
							newRowValues[coll] = item;
						}
					}

					var newRow = newDt.Rows.Add();
					newRow.ItemArray = newRowValues;

				}
				catch (Exception)
				{ }//

			}//rows cycle
		}//FillData

		public static DataTable GetItems(DataTable table, int offset, int count)
		{
			DataTable tableRes = new DataTable();

			tableRes.TableName = table.TableName;
			//tableRes.Columns.AddRange(table.Columns.Cast<DataColumn>().ToArray());

			foreach (DataColumn coll in table.Columns)
			{
				string name = coll.ColumnName;
				Type type = coll.DataType;
				DataColumn newColl = new DataColumn(name, type);
				tableRes.Columns.Add(newColl);
			}

			int start = offset;
			int length = Math.Min(table.Rows.Count, start + count);

			for (int i = start; i < length; i++)
			{
				object[] data = table.Rows[i].ItemArray;
				tableRes.Rows.Add(data);
			}
			return tableRes;
		}//Get items
	}
}
