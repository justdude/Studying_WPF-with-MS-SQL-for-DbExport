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
using DBExport.Common.Containers;
using DBExport.Common.MVVM;
using GalaSoft.MvvmLight.Messaging;
using DBExport.Common.Constants;

namespace DbExport.Controls.GeneratableForm
{
	internal class DataBox
	{
		public string Name { get; set; }
	}

	/// <summary>
	/// Логика взаимодействия для ctrGeneratableForm.xaml
	/// </summary>
	public partial class ctrGeneratableForm : UserControl
	{
		const string tbStyleName = "";
		const string tboxStyleName = "";
		const string tboxTmlErrorName = "tbError";

		public ctrGeneratableForm()
		{
			InitializeComponent();
			DataBoxes = new Dictionary<DataBox, FrameworkElement>();
			Loaded += ctrGeneratableForm_Loaded;
			Unloaded += ctrGeneratableForm_Unloaded;
		}

		public void Init(string token)
		{
			Token = token;

			Messenger.Default.Unregister(this);

			Messenger.Default.Register<DBExport.Common.Messages.LoadCollumnsMessage>(this, Token, OnBuildColumns);
			Messenger.Default.Register<DBExport.Common.Messages.LoadRowsMessage>(this, Token, OnRowsChanged);

		}

		//public string Token
		//{
		//	get { return (string)GetValue(TokenProperty); }
		//	set
		//	{
		//		SetValue(TokenProperty, value);
		//		Init();
		//	}
		//}

		//public static readonly DependencyProperty TokenProperty =
		//	DependencyProperty.Register("Token", typeof(string), typeof(ctrGeneratableForm), new PropertyMetadata(string.Empty));
		public string Token
		{
			get;
			set;
			//get
			//{
			//	return ControlBehavior.GetToken(this);
			//}
		}
		public ObservableCollection<CCollumnItem> Collumns { get; set; }
		private Dictionary<DataBox, FrameworkElement> DataBoxes { get; set; }

		void ctrGeneratableForm_Loaded(object sender, RoutedEventArgs e)
		{
		}

		void ctrGeneratableForm_Unloaded(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Unregister(this);
		}

		private void OnBuildColumns(DBExport.Common.Messages.LoadCollumnsMessage obj)
		{
			if (obj.Collumns == null)
				return;

			if (Collumns == null)
				Collumns = new ObservableCollection<CCollumnItem>();

			Collumns.Clear();

			foreach (var item in obj.Collumns)
			{
				Collumns.Add(item);
			}

			BuildCollumns();
		}

		private void OnRowsChanged(DBExport.Common.Messages.LoadRowsMessage obj)
		{
			foreach (var item in DataBoxes)
			{
				CRowItemViewModel viewModel = obj.Rows.FirstOrDefault(p => p.CollumnName == item.Key.Name);

				if (viewModel == null)
					continue;

				FrameworkElement tb = item.Value;
				tb.DataContext = viewModel;

				if (tb is TextBox)
				{
					SetTextBoxBinding(tb as TextBox, viewModel.ValueType, viewModel);
				}
				if (tb is DatePicker)
				{
					SetDatePickerBinding(tb as DatePicker, viewModel.ValueType, viewModel);
				}
				if (tb is ComboBox)
				{
					SetComboBoxBinding(tb as ComboBox, viewModel.ValueType, viewModel);
				}

			}

		}

		private void SetComboBoxBinding(ComboBox datePicker, Type type, object source)
		{
			var binding = new Binding();
			binding.Source = source;
			binding.Mode = BindingMode.TwoWay;

			if (type != null)
			{
				ValidationRule rule = null;

				string prName = string.Empty;

				prName = GetTypeName(type, prName);

				if (!string.IsNullOrWhiteSpace(prName))
				{
					binding.Path = new PropertyPath(prName);
					binding.Source = source;
					binding.ValidatesOnDataErrors = true;
					binding.NotifyOnValidationError = true;
					binding.Mode = BindingMode.TwoWay;
					binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

					if (rule != null)
					{
						binding.ValidationRules.Add(rule);
					}
				}
			}

			BindingOperations.SetBinding(datePicker, ComboBox.SelectedItemProperty, binding);
		}

		private void SetDatePickerBinding(DatePicker datePicker, Type type, object source)
		{
			var binding = new Binding();
			binding.Source = source;
			binding.Mode = BindingMode.TwoWay;

			if (type != null)
			{
				ValidationRule rule = null;

				string prName = string.Empty;

				prName = GetTypeName(type, prName);

				if (!string.IsNullOrWhiteSpace(prName))
				{
					binding.Path = new PropertyPath(prName);
					binding.Source = source;
					binding.ValidatesOnDataErrors = true;
					binding.NotifyOnValidationError = true;
					binding.Mode = BindingMode.TwoWay;
					binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

					if (rule != null)
					{
						binding.ValidationRules.Add(rule);
					}
				}
			}

			BindingOperations.SetBinding(datePicker, DatePicker.SelectedDateProperty, binding);
		}

		private static string GetTypeName(Type type, string prName)
		{
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					prName = TypeNames.BoolValue;
					break;
				case TypeCode.DateTime:
					prName = TypeNames.DateValue;
					break;
				case TypeCode.Single:
				case TypeCode.Double:
					prName = TypeNames.FloatValue;
					break;
				case TypeCode.Int32:
					prName = TypeNames.IntValue;
					break;
				case TypeCode.String:
					prName = TypeNames.StrValue;
					break;
				default:
					break;
			}
			return prName;
		}

		public void BuildCollumns()
		{
			DataBoxes.Clear();

			try
			{
				for (int i = 0; i < Collumns.Count; i++)
				{
					var row = CreateRowDefinition();
					grdData_Container.RowDefinitions.Add(row);

					var textBlock = CreateTextBlock(i, 0, Collumns[i].Name);
					FrameworkElement textBox = null;

					switch (Type.GetTypeCode(Collumns[i].ItemType))
					{
						case TypeCode.Boolean:
							textBox = CreateTrueFalseCombo(i, 1);
							break;
						case TypeCode.DateTime:
							textBox = CreateDatePicker(i, 1);
							break;
	
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Int32:
						case TypeCode.String:
							textBox = CreateTextBox(i, 1);
							break;
						default:
							break;
					}

					
					//SetTextBoxBinding(textBox, Collumns[i].ItemType);
					var dbox = new DataBox() { Name = Collumns[i].Name };
					DataBoxes.Add(dbox, textBox);

					grdData_Container.Children.Add(textBlock);
					grdData_Container.Children.Add(textBox);
				}
			}
			catch (Exception ex)
			{

			}
		}

		private ComboBox CreateTrueFalseCombo(int row, int column)
		{
			ComboBox tb = new ComboBox();

			tb.Height = 22;
			tb.Width = 150;
			tb.FontWeight = FontWeights.Bold;
			tb.Margin = new Thickness(5);
			//tb.Style = GetStyle(tboxStyleName);
			//SetErrorTemplate(tb);

			var bc = new BrushConverter();
			tb.Foreground = (Brush)bc.ConvertFrom("#FF2D72BC");

			Grid.SetColumn(tb, column);
			Grid.SetRow(tb, row);

			tb.Items.Add(true);
			tb.Items.Add(false);

			return tb;
		}

		private DatePicker CreateDatePicker(int row, int column)
		{
			DatePicker tb = new DatePicker();

			tb.Height = 22;
			tb.Width = 150;
			tb.FontWeight = FontWeights.Bold;
			tb.Margin = new Thickness(5);
			//tb.Style = GetStyle(tboxStyleName);
			//SetErrorTemplate(tb);

			var bc = new BrushConverter();
			tb.Foreground = (Brush)bc.ConvertFrom("#FF2D72BC");

			Grid.SetColumn(tb, column);
			Grid.SetRow(tb, row);

			return tb;
		}


		#region Instatiates
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
			//tb.Style = GetStyle(tbStyleName);

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
			//tb.Style = GetStyle(tboxStyleName);
			SetErrorTemplate(tb);

			var bc = new BrushConverter();
			tb.Foreground = (Brush)bc.ConvertFrom("#FF2D72BC");

			Grid.SetColumn(tb, column);
			Grid.SetRow(tb, row);

			return tb;
		}

		private void SetErrorTemplate(TextBox tb)
		{
			try
			{
				Validation.SetErrorTemplate(tb, this.FindResource(tboxTmlErrorName) as ControlTemplate);
			}
			catch (Exception ex)
			{ }
		}

		private static void SetTextBoxBinding(TextBox tb, Type type, object source)
		{
			var binding = new Binding();
			binding.Source = source;
			binding.Mode = BindingMode.TwoWay;

			if (type != null)
			{
				ValidationRule rule = null;

				string prName = string.Empty;

				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Boolean:
						prName = TypeNames.BoolValue;
						break;
					case TypeCode.DateTime:
						prName = TypeNames.DateValue;
						break;
					case TypeCode.Single:
					case TypeCode.Double:
						prName = TypeNames.FloatValue;
						break;
					case TypeCode.Int32:
						prName = TypeNames.IntValue;
						break;
					case TypeCode.String:
						prName = TypeNames.StrValue;
						break;
					default:
						break;
				}

				if (!string.IsNullOrWhiteSpace(prName))
				{
					binding.Path = new PropertyPath(prName);
					binding.Source = source;
					binding.ValidatesOnDataErrors = true;
					binding.NotifyOnValidationError = true;
					binding.Mode = BindingMode.TwoWay;
					binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

					if (rule != null)
					{
						binding.ValidationRules.Add(rule);
					}
				}
			}

			BindingOperations.SetBinding(tb, TextBox.TextProperty, binding);
		}

		private Style GetStyle(string name)
		{
			Style style = null;
			try
			{
				style = this.FindResource(name) as Style;
			}
			catch (Exception ex)
			{ }

			return style;
		}
		#endregion
	}
}
