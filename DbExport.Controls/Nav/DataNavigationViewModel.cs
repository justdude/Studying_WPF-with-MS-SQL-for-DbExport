using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBExport.Common.MVVM;
using GalaSoft.MvvmLight.Command;

namespace DbExport.Controls.Nav
{
	public class DataNavigationViewModel : ViewModelExtended
	{
		#region fields

		private int start = 0;
		private int itemCount = 0;
		private int totalItems = 100;

		private readonly RelayCommand firstCommand;
		private readonly RelayCommand previousCommand;
		private readonly RelayCommand nextCommand;
		private readonly RelayCommand lastCommand;

		#endregion

		#region Ctr
		public DataNavigationViewModel()
		{
			RaiseRefreshProperties();

			firstCommand = new RelayCommand(GoToFirst, CanGoToFirst);
			lastCommand = new RelayCommand(GoToLast, CanGoToLast);
			nextCommand = new RelayCommand(GoToNext, CanGoToNext);
			previousCommand = new RelayCommand(GoToPrevious, CanGoToPrevious);
		}
		#endregion

		#region Commands checkers

		private bool CanGoToFirst()
		{
			return start - itemCount >= 0 ? true : false;
		}

		private bool CanGoToNext()
		{
			return start + itemCount < totalItems ? true : false;
		}

		private bool CanGoToPrevious()
		{
			return start - itemCount >= 0 ? true : false;
		}

		private bool CanGoToLast()
		{
			return start + itemCount < totalItems ? true : false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the index of the first item in the products list.
		/// </summary>
		public int Start { get { return start + 1; } }

		/// <summary>
		/// Gets the index of the last item in the products list.
		/// </summary>
		public int End { get { return start + itemCount < totalItems ? start + itemCount : totalItems; } }

		/// <summary>
		/// The number of total items in the data store.
		/// </summary>
		public int TotalItems { get { return totalItems; } }

		/// <summary>
		/// Gets the command for moving to the first page of products.
		/// </summary>
		public RelayCommand FirstCommand
		{
			get
			{
				return firstCommand;
			}
		}

		/// <summary>
		/// Gets the command for moving to the previous page of products.
		/// </summary>
		public RelayCommand PreviousCommand
		{
			get
			{
				return previousCommand;
			}
		}

		/// <summary>
		/// Gets the command for moving to the next page of products.
		/// </summary>
		public RelayCommand NextCommand
		{
			get
			{
				return nextCommand;
			}
		}

		/// <summary>
		/// Gets the command for moving to the last page of products.
		/// </summary>

		public RelayCommand LastCommand
		{
			get
			{
				return lastCommand;
			}
		}

		#endregion

		#region Methods

		private void GoToLast()
		{

			start = (totalItems / itemCount - 1) * itemCount;
			start += totalItems % itemCount == 0 ? 0 : itemCount;

			RaiseRefreshProperties();
		}

		private void GoToFirst()
		{
			
			start = 0;
			RaiseRefreshProperties();
		}

		private void GoToPrevious()
		{
			start -= itemCount;

			RaiseRefreshProperties();
		}

		private void GoToNext()
		{
			start += itemCount;

			RaiseRefreshProperties();
		}

		/// <summary>
		/// Refreshes the list of products. Called by navigation commands.
		/// </summary>
		private void RaiseRefreshProperties()
		{
			//Products = DataAccess.GetProducts(start, itemCount, sortColumn, ascending, out totalItems);

			RaisePropertyChanged(() => this.Start);
			RaisePropertyChanged(() => this.End);
			RaisePropertyChanged(() => this.TotalItems);
		}
		#endregion

	}
}
