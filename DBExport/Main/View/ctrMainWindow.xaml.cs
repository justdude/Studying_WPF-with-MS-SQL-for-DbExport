using DbExport.Common;
using DbExport.CSV;
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
using DBExport.Main.ViewModel;

namespace DBExport.Main
{
	/// <summary>
	/// Логика взаимодействия для ctrMainWindow.xaml
	/// </summary>
	public partial class ctrMainWindow : UserControl
	{
		public ctrMainWindow()
		{
			InitializeComponent();
			this.Loaded += ctrMainWindow_Loaded;
		}

		void ctrMainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			var viewModel = new TablesHandleViewModel();
			viewModel.Disp = this.Dispatcher;

			DataContext = viewModel;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
