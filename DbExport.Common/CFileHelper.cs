using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbExport.Common
{
	public class CFileHelper
	{
		public static string GetPathFromDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text files (*.csv)|*.csv";
			openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
			if (openFileDialog.ShowDialog() == DialogResult.OK || openFileDialog.ShowDialog() == DialogResult.Yes)
			{
				return openFileDialog.FileName;
			}
			return null;
		}
	}
}
