using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExport.Common.Interfaces;

namespace DbExport.Data
{
	public class CObjectBase : IObjectBase
	{
		#region Члены IObjectBase

		public Status Status
		{
			get;
			set;
		}

		public virtual bool Save()
		{
			//bool res = true;

			//try
			//{
			//	//save this
			//	switch (Status)
			//	{
			//		case Status.Added:
			//			this.Id = Generator.GenerateID();
			//			res = AddValue(this);
			//			Status = Common.Interfaces.Status.Normal;
			//			break;
			//		case Status.Normal:
			//			break;
			//		case Status.Updated:
			//			res = UpdateValue(this);
			//			Status = Common.Interfaces.Status.Normal;
			//			break;
			//		case Status.Deleted:
			//			res = DeleteValue(this.Id);
			//			break;
			//		default:
			//			Status = Common.Interfaces.Status.Normal;
			//			break;
			//	}
			//}
			//catch (Exception ex)
			//{
			//	res = false;
			//}

			//return res;
			return true;
		}


		public string Id
		{
			get;
			set;
		}

		#endregion
	}
}
