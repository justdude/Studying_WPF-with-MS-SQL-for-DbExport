using System;
using System.Collections.Generic;
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

namespace DbExport.Controls.Nav
{
	/// <summary>
	/// Логика взаимодействия для ctrDataNav.xaml
	/// </summary>
	public partial class ctrDataNav : UserControl
	{
		public ctrDataNav()
		{
			InitializeComponent();
			Loaded += ctrDataNav_Loaded;
		}

		public DataNavigationViewModel ViewModel { get; set; }

		void ctrDataNav_Loaded(object sender, RoutedEventArgs e)
		{
			ViewModel = new DataNavigationViewModel();
			ViewModel.Disp = this.Dispatcher;
			DataContext = ViewModel;
		}

	}
}
