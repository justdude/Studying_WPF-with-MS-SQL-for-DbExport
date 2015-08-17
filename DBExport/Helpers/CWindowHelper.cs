﻿using DBExport.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExport.Helpers
{
	public class CWindowHelper
	{
		public static void ShowEmployeWindow(out Dictionary<string, Type> selectedTypes)
		{
			selectedTypes = new Dictionary<string, Type>();
			var wind = new wndSetTypes();
			//wind.Owner = App.Current.MainWindow;
			//wind.Owner.Hide();
			//wind.Closed += (e, a) =>
			//{
			//	wind.Owner.Show();
			//	wind.Activate();
			//};
			wind.DataContext = new Settings.ViewModel.TableSettingViewModel(selectedTypes);
			wind.ShowDialog();
		}
	}
}