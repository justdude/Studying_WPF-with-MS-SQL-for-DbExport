using DbExport.Data;
using DBExport.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBExport.Products;
using DBExport.Filtering.ViewModel;
using System.Windows;

namespace DBExport.Helpers
{
	public class CWindowHelper
	{
		public static void ShowEmployeWindow(CTable table, Func<object, bool> checkRightColl, Action<bool> onWindowClosed)
		{
			var wind = new wndSetTypes();
			//wind.Owner = App.Current.MainWindow;
			//wind.Owner.Hide();
			//wind.Closed += (e, a) =>
			//{
			//	wind.Owner.Show();
			//	wind.Activate();
			//};
			wind.DataContext = new Settings.ViewModel.TableSettingViewModel(table, checkRightColl)
			{
				Token = wind.Token,
			};
			wind.Closed += (p, v) =>
				{
					if (onWindowClosed != null)
						onWindowClosed(wind.IsOk);
				};

			wind.Show();
		}

		public static void ShowEditSelectedTable(CTable table)
		{
			var wind = new wndProducts();
			wind.Owner = App.Current.MainWindow;
			wind.Owner.Hide();
			wind.Closed += (e, a) =>
			{
				wind.Owner.Show();
				wind.Activate();
			};

			wind.Loaded += (p, e) =>
			{
				wind.Init();
				wind.ctrProd.Init(wind.Token);
				var vm = new ProductItemsViewModel(table);
				wind.DataContext = vm;
				vm.Disp = wind.Dispatcher;
				vm.Token = wind.Token;
			};

			wind.Show();
		}

		public static void ShowSelectFilterWindow(CTable table, Action<CTable, List<string>> onFilterSelected)
		{
			var wind = new wndSelectFilter();
			wind.Owner = App.Current.Windows.Cast<Window>().FirstOrDefault(p => p.IsActive);
			wind.Closed += (e, a) =>
			{
				wind.Owner.Show();
				wind.Activate();
			};

			wind.Loaded += (p, e) =>
			{
				wind.DataContext = new SelectFilterViewModel(table, onFilterSelected)
				{
					Token = wind.Token,
					Disp = wind.Dispatcher
				};
			};

			wind.ShowDialog();
		}
	}
}
