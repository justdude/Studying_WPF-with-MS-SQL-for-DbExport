using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBExport.Products.View
{
	/// <summary>
	/// Логика взаимодействия для ctrProductWindow.xaml
	/// </summary>
	public partial class ctrProductWindow : UserControl
	{
		public ctrProductWindow()
		{
			InitializeComponent();
		}

		//private void Button_Click(object sender, RoutedEventArgs e)
		//{
		//	DataRowView rowView = (DataRowView)grdTable.SelectedItem;
		//	OnItemChanged(rowView, DbExport.Common.Interfaces.Status.Deleted);
		//}

		//private void OnItemChanged(DataRowView rowView, DbExport.Common.Interfaces.Status status)
		//{
		//	rowView.Delete();
		//}
	}
}
