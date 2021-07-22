using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Runner.ViewModels.Abstracts
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		protected void TriggerPropertyChanged(string arg1)
		{
			if (this.PropertyChanged is not null)
			{
				// Trigger View Update.
				this.PropertyChanged(this, new PropertyChangedEventArgs(arg1));
			}
		}

		protected void TriggerPropertyChanged(string arg1, string arg2)
		{
			if (this.PropertyChanged is not null)
			{
				// Trigger View Update.
				this.PropertyChanged(this, new PropertyChangedEventArgs(arg1));
				this.PropertyChanged(this, new PropertyChangedEventArgs(arg2));
			}
		}

		protected void TriggerPropertyChanged(string arg1, string arg2, string arg3)
		{
			if (this.PropertyChanged is not null)
			{
				// Trigger View Update.
				this.PropertyChanged(this, new PropertyChangedEventArgs(arg1));
				this.PropertyChanged(this, new PropertyChangedEventArgs(arg2));
				this.PropertyChanged(this, new PropertyChangedEventArgs(arg3));
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members
	}
}
