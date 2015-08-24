using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Database;
using DbExport.Common.Interfaces;
using DbExport.Database;
using DbExport.Data.Constants;

namespace DbExport.Data
{
	public class CTable : CObjectBase
	{
		private string mvName;

		public CTable()
		{
			Columns = new List<CColumn>();
			Rows = new List<CValue>();
		}

		public string Name
		{
			get
			{
				return mvName;
			}
			set
			{
				mvName = value;
			}
		}

		public DataTable Data { get; set; }

		public List<CColumn> Columns { get; set; }
		public List<CValue> Rows { get; set; }

		public CTable CreateDummy()
		{
			CTable table = new CTable();
			table.Id = Generator.GenerateID();
			table.Name = "Table" + DateTime.Now.Minute;

			table.Columns = new List<CColumn>();
			string[] Types = { "string", "int", "float", "bool", "DateTime" };

			for (int i = 0; i < Types.Length; i++)
			{
				var coll = new CColumn();
				coll.Id = Generator.GenerateID();
				coll.TableId = table.Id;

				coll.Name = "Coll" + i;
				coll.CollType = Types[i];

				table.Columns.Add(coll);
			}

			for (int i = 0; i < table.Columns.Count; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					//table.Columns[i]
					CValue value = new CValue();
					value.Id = Generator.GenerateID();
					value.TableId = table.Id;

					//value.SetValue((object)nul);
				}
			}

			return table;
		}

		#region Converters

		public static DataTable ToDataTable(CTable table)
		{
			DataTable dataTable = new DataTable(table.Name);

			//dataTable.Columns = new DataColumnCollection();
			//dataTable.Rows = new DataRowCollection();
			try
			{
				for (int coll = 0; coll < table.Columns.Count; coll++)
				{
					dataTable.Columns.Add(table.Columns[coll].Name, table.Columns[coll].GetCollType());
				}

				List<List<CValue>> list = table.Rows.GroupBy(p => p.RowNumb).Select(p => p.ToList()).ToList();

				foreach (List<CValue> collection in list)
				{
					object[] data = new object[dataTable.Columns.Count];
					for (int coll = 0; coll < dataTable.Columns.Count; coll++)
					{
						CValue item = collection.FirstOrDefault(p => p.CollumnId == table.Columns[coll].Id);

						if (item == null)
							continue;

						data[coll] = item.GetValue();
					}

					var newRow = dataTable.Rows.Add();
					newRow.ItemArray = data;
				}
			}
			catch(Exception ex)
			{

			}


			return dataTable;
		}

		
		public static CTable ToDbTable(DataTable dataTable)
		{
			if (dataTable == null)
				return null;

			CTable table = new CTable();

			table.Id = Generator.GenerateID();
			table.Name = dataTable.TableName;

			table.Data = dataTable;
			table.Status = Status.Added;

			table.Columns = new List<CColumn>();
			table.Rows = new List<CValue>();

			FillColumns(dataTable, table);
			FillRows(dataTable, table);

			return table;

		}

		private static void FillRows(DataTable dataTable, CTable table)
		{
			try
			{
				for (int row = 0; row < dataTable.Rows.Count; row++)
				{
					CValue value = null;

					for (int coll = 0; coll < dataTable.Columns.Count; coll++)
					{
						value = new CValue();
						value.Id = Generator.GenerateID();
						value.TableId = table.Id;

						value.SetValue(dataTable.Rows[row].ItemArray[coll]);

						value.ValueType = table.Columns[coll].GetCollType();
						value.Column = table.Columns[coll];

						value.Status = Status.Added;

						table.Rows.Add(value);
					}
				}
			}
			catch (Exception ex)
			{ }
		}

		private static void FillColumns(DataTable dataTable, CTable table)
		{
			try
			{
				for (int i = 0; i < dataTable.Columns.Count; i++)
				{
					var coll = new CColumn();
					coll.Id = Generator.GenerateID();
					coll.TableId = table.Id;

					coll.Name = dataTable.Columns[i].ColumnName;
					coll.CollType = null;

					coll.Status = Status.Added;

					table.Columns.Add(coll);
				}
			}
			catch (Exception ex)
			{ }
		}
		#endregion

		#region Члены IObjectBase

		public override bool Save()
		{
			bool res = true;

			//save this
			switch (Status)
			{
				case Status.Added:
					this.Id = Generator.GenerateID();
					res = AddTable(this);
					break;
				case Status.Normal:
					break;
				//case Status.Updated:
				//	res = UpdateTable(this);
				//	break;
				//case Status.Deleted:
				//	res = DeleteTable(this);
				//break;
				default:
					Status = Common.Interfaces.Status.Normal;
					break;
			}

			if (res)
			{
				Status = Common.Interfaces.Status.Normal;
			}

			res &= Columns.SaveList(Status);
			res &= Rows.SaveList(Status);

			return true;
		}

		private bool AddTable(CTable item)
		{
			string str = modSQL.InsertTable(item);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		private bool UpdateTable(CTable item)
		{
			string str = modSQL.UpdateTable(item);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		private bool DeleteTable(string ud)
		{
			string str = modSQL.DeleteTable(ud);
			return CDatabase.Instance.ExecuteNonQuery(str);
		}

		#endregion

		#region Table init
		
		public static CTable Create(DataTable data)
		{
			return CTable.ToDbTable(data);
		}

		#endregion

		public void LoadData()
		{
			//if (Status == Common.Interfaces.Status.Added && this.Data != null)
			//{
			//	return;
			//}

		}
	}
}
