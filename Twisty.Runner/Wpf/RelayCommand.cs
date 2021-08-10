using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Twisty.Runner.Wpf
{
	/// <summary>
	/// Standard COmmand object used to relay WPF Commands.
	/// </summary>
	public class RelayCommand : ICommand
	{
		private Action<object> m_Execute;
		private Predicate<object> m_CanExecute;

		/// <summary>
		/// Create a new RelayCommand object for the specified delegate methods.
		/// </summary>
		/// <param name="execute">Action performing the Command execution.</param>
		/// <param name="canExecute">Predicate indicating if the Command can be executed.</param>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			this.m_Execute = execute;
			this.m_CanExecute = canExecute;
		}

		/// <summary>
		/// Trigger the CanExecuteChanged event on this command.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged is not null)
				CanExecuteChanged(this, EventArgs.Empty);
		}

		#region ICommand Members

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return this.m_CanExecute is null || this.m_CanExecute(parameter);
		}

		public void Execute(object parameter)
		{
			this.m_Execute(parameter);
		}

		#endregion ICommand Members
	}
}