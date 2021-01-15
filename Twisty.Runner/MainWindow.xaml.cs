using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Twisty.Engine.Geometry;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;
using Twisty.Engine.Structure.Skewb;
using Twisty.Runner.Views;

namespace Twisty.Runner
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private RubikCube m_Core;
		private List<RotationCoreStandardView> m_Views;

		public MainWindow()
		{
			InitializeComponent();

			m_Core = new RubikCube(3);
			//SkewbCube c = new SkewbCube();

			m_Views = new List<RotationCoreStandardView>();
			m_Views.Add(this.CubeView1);
			m_Views.Add(this.CubeView2);
			m_Views.Add(this.CubeView3);
			m_Views.Add(this.CubeView4);

			foreach (var v in m_Views)
				v.Core = m_Core;

			this.ConsoleInput.RunAlgorithm += ConsoleInput_RunAlgorythm;
		}

		private void ConsoleInput_RunAlgorythm(string command)
		{
			var p = new RubikOperationsParser();
			var operations = p.Parse(command);

			foreach (var o in operations)
				o.ExecuteOn(m_Core);

			foreach (var v in m_Views)
				v.Refresh();
		}
	}
}
