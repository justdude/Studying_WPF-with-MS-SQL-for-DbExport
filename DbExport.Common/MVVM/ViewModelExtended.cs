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
		private string mvToken;
		private bool mvIsVisible;
		private string mvHelpState;

		public ViewModelExtended()
		{
			mvIsVisible = true;
		}

		public string Token
		{
			get
			{
				return mvToken;
			}
			set
			{
				mvToken = value;
				OnTokenChanged();
			}
		}

		public bool IsVisible
		{
			get
			{
				return mvIsVisible;
			}
			set
			{
				if (mvIsVisible == value)
					return;

				mvIsVisible = value;
				RaisePropertyChanged(() => this.IsVisible);
			}
		}

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

		public virtual string StateText
		{
			get
			{
				return mvHelpState;
			}
			set
			{
				if (mvHelpState == value)
					return;

				mvHelpState = value;

				this.RaisePropertyChanged(() => this.StateText);
			}
		}

		protected virtual void Translate()
		{ 
		
		}

		protected virtual void OnTokenChanged()
		{
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
