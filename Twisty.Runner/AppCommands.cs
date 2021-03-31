using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Twisty.Runner.Wpf;

namespace Twisty.Runner
{
	/// <summary>
	/// Static class providing all general application commands.
	/// </summary>
	public class AppCommands
	{
		/// <summary>
		/// Command Exiting the application.
		/// </summary>
		public ICommand Exit { get; } = new RelayCommand((p) => App.Current.Shutdown());
	}
}
