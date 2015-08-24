using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DBExport.Common.MVVM
{
	public class ViewModelExtended : ViewModelBase
	{
		public string Token { get; set; }

		public Dispatcher Disp { get; set; }

		public void BeginInvoke(DispatcherPriority priority, Action action)
		{
			if (Disp == null)
				return;

			Disp.BeginInvoke(action, priority, null);
		}
	}
}
