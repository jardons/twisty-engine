using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Twisty.Engine.Geometry;
using Twisty.Engine.Materialization;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Structure;
using Twisty.Runner.Wpf;

namespace Twisty.Runner.Views
{
	public delegate void TriggerCommand(string command);

	/// <summary>
	/// Control allowing to inputs algorithms.
	/// </summary>
	public partial class AlgorithmConsole : UserControl
	{
		public AlgorithmConsole()
		{
			InitializeComponent();

			this.Input.KeyUp += OnInputKeyUp;
			this.ButtonExecute.Click += OnExecute;
			this.History.MouseDoubleClick += OnDoubleClickHistory;
		}

		#region Events

		public event TriggerCommand RunAlgorithm;

		#endregion Events

		private void ExecuteCommand()
			=> ExecuteCommand(this.Input.Text);

		private void ExecuteCommand(string command)
		{
			if (string.IsNullOrWhiteSpace(command))
				return;

			this.RunAlgorithm(command);
			this.AddToHistory(command);
			this.Input.Clear();
		}

		private void AddToHistory(string command)
		{
			if (string.IsNullOrWhiteSpace(command))
				return;

			var item = this.History.Items.OfType<ListBoxItem>().FirstOrDefault(i => i.Content.ToString() == command);

			// If item is already in history, avoid duplicate.
			if (item != null)
				this.History.Items.Remove(item);

			// Add at the bottom of history.
			this.History.Items.Add(new ListBoxItem { Content = command });
		}

		private void OnInputKeyUp(object sender, KeyEventArgs e)
		{
			// Enter key trigger execution
			if (e.Key == Key.Enter)
			{
				ExecuteCommand();
				return;
			}

			var parser = new RubikOperationsParser();

			if (parser.TryClean(this.Input.Text, out string command))
				this.Input.Text = command;
		}

		private void OnExecute(object sender, RoutedEventArgs e)
			=> ExecuteCommand();

		private void OnDoubleClickHistory(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.ExecuteCommand(((ListBoxItem)this.History.SelectedItem).Content.ToString());
			}
		}
	}
}
