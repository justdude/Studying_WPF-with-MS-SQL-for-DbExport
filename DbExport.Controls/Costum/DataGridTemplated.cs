using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace DBExport.Common.Controls
{
	public class DataGridTemplated : DataGrid
	{
		public DataTemplateSelector CellTemplateSelector
		{
			get { return (DataTemplateSelector)GetValue(CellTemplateSelectorProperty); }
			set { SetValue(CellTemplateSelectorProperty, value); }
		}

		public static readonly DependencyProperty CellTemplateSelectorProperty =
			DependencyProperty.Register("Selector", typeof(DataTemplateSelector), typeof(DataGridTemplated),
			new FrameworkPropertyMetadata(null));

		public string Token { get; set; }

		public DataGridTemplated():base()
		{
			Loaded += DataGridTemplated_Loaded;
		}

		void DataGridTemplated_Loaded(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Register<Messages.DeleteSelected>(this, Token, OnDeleteSelected);
			Messenger.Default.Register<Messages.ChangeSelected>(this,  Token, OnChangeSelected);
		}

		private void OnDeleteSelected(Messages.DeleteSelected obj)
		{
		}

		private void OnChangeSelected(Messages.ChangeSelected obj)
		{
		}

		protected override void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
		{
			e.Cancel = CellTemplateSelector == null;
			Columns.Add(new DataGridTemplateColumn
			{
				Header = e.Column.Header,
				CellTemplateSelector = CellTemplateSelector
			});
			base.OnAutoGeneratingColumn(e);
		}
	}
}
