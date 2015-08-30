using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DbExport.Controls.GeneratableForm
{
	/// <summary>
	/// Логика взаимодействия для ctrGeneratableForm.xaml
	/// </summary>
	public partial class ctrGeneratableForm : UserControl
	{
		private string tbStyleName;
		private string tboxStyleName;

		public ctrGeneratableForm()
		{
			InitializeComponent();
			Items = new Dictionary<string, TextBox>();
		}

		public ObservableCollection<CCollumnItem> Collumns { get; set; }
		public Dictionary<string, TextBox> Items { get; private set; }

		public void BuildCollumns()
		{
			Items.Clear();

			for (int i = 0; i < Collumns.Count; i++)
			{
				var row = CreateRowDefinition();
				grdData_Container.RowDefinitions.Add(row);

				var textBlock = CreateTextBlock(i, 0, Collumns[i].Name);
				var textBox = CreateTextBox(i, 1);
				SetTextBoxBinding(textBox, "Value", Collumns[i].ItemType);

				Items.Add(Collumns[i].Name, textBox);

				grdData_Container.Children.Add(textBlock);
				grdData_Container.Children.Add(textBox);
			}
		}



		private RowDefinition CreateRowDefinition()
		{
			RowDefinition RowDefinition = new RowDefinition();
			RowDefinition.Height = GridLength.Auto;
			return RowDefinition;
		}

		private TextBlock CreateTextBlock(int row, int column, string text)
		{
			TextBlock tb = new TextBlock() { Text = text };
			tb.MinWidth = 90;
			tb.FontWeight = FontWeights.Bold;
			tb.Margin = new Thickness(5);
			tb.Style = GetStyle(tbStyleName);

			var bc = new BrushConverter();
			tb.Foreground = (Brush)bc.ConvertFrom("#FF2D72BC");

			Grid.SetColumn(tb, column);
			Grid.SetRow(tb, row);
			
			return tb;
		}

		private TextBox CreateTextBox(int row, int column)
		{
			TextBox tb = new TextBox();

			tb.Height = 22;
			tb.Width = 150;
			tb.FontWeight = FontWeights.Bold;
			tb.Margin = new Thickness(5);
			tb.Style = GetStyle(tboxStyleName);

			var bc = new BrushConverter();
			tb.Foreground = (Brush)bc.ConvertFrom("#FF2D72BC");

			Grid.SetColumn(tb, column);
			Grid.SetRow(tb, row);

			return tb;
		}

		private static void SetTextBoxBinding(TextBox tb, string propName, Type type)
		{
			var binding = new Binding(propName);
			binding.Source = tb;
			binding.Mode = BindingMode.TwoWay;

			if (type != null)
			{
				ValidationRule rule = null;

				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Boolean:
						break;
					case TypeCode.DateTime:
						break;
					case TypeCode.Double:
						break;
					case TypeCode.Int32:
						break;
					case TypeCode.String:
						break;
					case TypeCode.Single:
						break;
					default:
						break;
				}

				if (rule != null)
				{
					binding.NotifyOnValidationError = true;
					binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
					binding.ValidationRules.Add(rule);
				}
			}

			BindingOperations.SetBinding(tb, TextBox.TextProperty, binding);
		}

		private Style GetStyle(string name)
		{
			Style style = this.FindResource(name) as Style;
			return style;
		}

	}
}
