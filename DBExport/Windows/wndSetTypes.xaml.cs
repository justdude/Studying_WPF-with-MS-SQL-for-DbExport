using DBExport.Common.Messages;
using DBExport.Common.MVVM;
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
using System.Windows.Shapes;

namespace DBExport.Windows
{
	/// <summary>
	/// Interaction logic for wndSetTypes.xaml
	/// </summary>
	public partial class wndSetTypes : MetroWindow
	{
		public wndSetTypes()
		{
			InitializeComponent();
			this.Loaded += wndSetTypes_Loaded;
			this.Closed += wndSetTypes_Closed;
		}

		void wndSetTypes_Loaded(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Register<CloseWindowMessage>(this, Token, OnMessageCloseWind);
		}

		private void OnMessageCloseWind(CloseWindowMessage obj)
		{
			this.Close();
		}

		void wndSetTypes_Closed(object sender, EventArgs e)
		{
			Messenger.Default.Unregister(this);
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
