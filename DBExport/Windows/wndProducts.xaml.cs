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
using System.Windows.Shapes;
using DBExport.Common.MVVM;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;

namespace DBExport.Windows
{
	/// <summary>
	/// Логика взаимодействия для wndProducts.xaml
	/// </summary>
	public partial class wndProducts : MetroWindow
	{
		public wndProducts()
		{
			InitializeComponent();
			InitMessenger();
			Loaded += wndProducts_Loaded;
			Unloaded += wndProducts_Unloaded;
		}

		private void InitMessenger()
		{
			Messenger.Default.Register<DBExport.Common.Messages.CloseWindowMessage>(Token, OnWindowClose);
		}

		public string Token
		{
			get
			{
				return ControlBehavior.GetToken(this);
			}
		}

		void wndProducts_Unloaded(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Unregister(this);
		}

		void wndProducts_Loaded(object sender, RoutedEventArgs e)
		{

			//var viewModel = new TablesHandleViewModel();
			//viewModel.Disp = this.Dispatcher;

			//DataContext = viewModel;
		}

		private void OnWindowClose(Common.Messages.CloseWindowMessage obj)
		{
			Application.Current.Shutdown();
		}

	}
}
