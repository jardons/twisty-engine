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
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;

namespace Twisty.Runner
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			RubikCube c = new RubikCube(3);

			this.CubeView1.Core = c;
			this.CubeView2.Core = c;
			this.CubeView3.Core = c;
			this.CubeView4.Core = c;

			c.RotateAround(c.GetAxis("R"), true);
			c.RotateAround(c.GetAxis("U"), true);
			//c.RotateAround(c.GetAxis("L"), true);
		}
    }
}
