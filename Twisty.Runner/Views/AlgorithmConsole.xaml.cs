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
		private RotationCore m_Core;

		public AlgorithmConsole()
		{
			InitializeComponent();

			this.Input.KeyUp += OnInputKeyUp;
			this.ButtonExecute.Click += OnExecute;
		}

		#region Events

		public event TriggerCommand RunAlgorithm;

		#endregion Events

		private void ExecuteCommand()
		{
			this.RunAlgorithm(this.Input.Text);
			this.Input.Clear();
		}

		private void OnInputKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				ExecuteCommand();
		}

		private void OnExecute(object sender, RoutedEventArgs e)
			=> ExecuteCommand();
	}
}
