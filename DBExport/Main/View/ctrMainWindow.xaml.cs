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
using DBExport.Common.MVVM;
using GalaSoft.MvvmLight.Messaging;

namespace DBExport.Main
{

	internal class BindableValue
	{
		public object Value { get; set; }
	}

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

		public string Token
		{
			get
			{
				return ControlBehavior.GetToken(this);
			}
		}

		void ctrMainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Register<DBExport.Common.Messages.LoadTableViewMessage>(this, Token, OnLoadTableViewMessage); 
		}

		private void OnLoadTableViewMessage(Common.Messages.LoadTableViewMessage obj)
		{
			dtgrSelectedTableView.Columns.Clear();

			for (int i = 0; i < obj.Data.Columns.Count; i++)
			{
				GridViewColumn coll = new GridViewColumn();//DataGridTextColumn();
				coll.Header = obj.Data.Columns[i].ColumnName;
				coll.Width = 100;
				coll.DisplayMemberBinding = new Binding(obj.Data.Columns[i].ColumnName);

				dtgrSelectedTableView.Columns.Add(coll);
			}

			Binding bind = new Binding();
			lstView.DataContext = obj.Data;

			lstView.SetBinding(ListView.ItemsSourceProperty, bind);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
