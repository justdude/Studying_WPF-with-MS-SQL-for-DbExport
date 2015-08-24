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
		private Dispatcher mvDispatcher;
		public string Token { get; set; }

		public Dispatcher Disp 
		{
			get
			{
				return mvDispatcher;
			}
			set
			{
				mvDispatcher = value;
				RaiseOnDispatcherChaned(mvDispatcher);
			}
		}

		public Action<Dispatcher> OnDispatcherChanged { get; set; }

		private void RaiseOnDispatcherChaned(Dispatcher dispatcher)
		{
			if (OnDispatcherChanged == null)
				return;

			OnDispatcherChanged(dispatcher);
		}

		public void BeginInvoke(DispatcherPriority priority, Action action)
		{
			if (Disp == null)
				return;

			Disp.BeginInvoke(action, priority, null);
		}

		public void Invoke(DispatcherPriority priority, Action action)
		{
			if (Disp == null)
				return;

			Disp.Invoke(action, priority, null);
		}
	}
}
