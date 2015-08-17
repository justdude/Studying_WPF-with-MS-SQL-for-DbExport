using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
