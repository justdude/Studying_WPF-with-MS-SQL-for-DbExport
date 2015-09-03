using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DBExport.Common.MVVM
{

	/// <summary>
	/// An attached property for supporting listbox selected items
	/// </summary>
	public class ListBoxBehaviour
	{
		#region SelectedItems

		private static ListBox list;
		private static bool _isRegisteredSelectionChanged = false;

		///
		/// SelectedItems Attached Dependency Property
		///
		public static readonly DependencyProperty SelectedItemsProperty =
		DependencyProperty.RegisterAttached("SelectedItems", typeof(IList),
		typeof(ListBoxBehaviour),
		new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
		new PropertyChangedCallback(OnSelectedItemsChanged)));

		public static IList GetSelectedItems(DependencyObject d)
		{
			return (IList)d.GetValue(SelectedItemsProperty);
		}

		public static void SetSelectedItems(DependencyObject d, IList value)
		{
			d.SetValue(SelectedItemsProperty, value);
		}

		private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!_isRegisteredSelectionChanged)
			{
				var listBox = (ListBox)d;
				list = listBox;
				listBox.SelectionChanged += listBox_SelectionChanged;
				_isRegisteredSelectionChanged = true;
			}
		}

		private static void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//Get list box's selected items.
			IEnumerable listBoxSelectedItems = list.SelectedItems;
			//Get list from model
			IList ModelSelectedItems = GetSelectedItems(list);

			//Update the model
			ModelSelectedItems.Clear();

			if (list.SelectedItems != null)
			{
				foreach (var item in list.SelectedItems)
					ModelSelectedItems.Add(item);
			}
			SetSelectedItems(list, ModelSelectedItems);
		}
		#endregion
	}
}
