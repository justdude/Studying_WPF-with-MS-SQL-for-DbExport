using DBExport.Common.MVVM;
using DBExport.Main.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
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

namespace DBExport
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
			Unloaded += MainWindow_Unloaded;
		}

		void MainWindow_Unloaded(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Unregister(this);
		}

		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Register<DBExport.Common.Messages.CloseWindowMessage>(Token, OnWindowClose);

			var viewModel = new TablesHandleViewModel();
			viewModel.Disp = this.Dispatcher;

			DataContext = viewModel;
		}

		private void OnWindowClose(Common.Messages.CloseWindowMessage obj)
		{
			Application.Current.Shutdown();
		}

		public string Token
		{
			get
			{
				return ControlBehavior.GetToken(this);
			}
		}
	}
}
