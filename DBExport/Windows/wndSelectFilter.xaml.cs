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
using DBExport.Common.Messages;
using DBExport.Common.MVVM;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;

namespace DBExport.Windows
{
	/// <summary>
	/// Логика взаимодействия для wndSelectFilter.xaml
	/// </summary>
	public partial class wndSelectFilter : MetroWindow
	{
		public wndSelectFilter()
		{
			InitializeComponent();
			Loaded += wndSelectFilter_Loaded;
			Closed += wndSelectFilter_Closed;
		}

		public string Token
		{
			get
			{
				return ControlBehavior.GetToken(this);
			}
		}

		void wndSelectFilter_Closed(object sender, EventArgs e)
		{
			Messenger.Default.Unregister(this);
		}

		void wndSelectFilter_Loaded(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Register<CloseWindowMessage>(this, Token, OnMessageCloseWind);
		}

		private void OnMessageCloseWind(CloseWindowMessage obj)
		{
			this.Close();
		}
	}
}
