using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Twisty.Engine.Materialization;
using Twisty.Runner.Views;
using Twisty.Runner.Wpf;

namespace Twisty.Runner.Models.Model3d
{
	public class Core3dSurface
	{
		public Core3dSurface(MaterializedObjectPart p)
		{
			this.FrontColor = GetColor(p.Color);
			this.BackColor = GetColor(p.Color);
			this.Points = p.Points.Select(p => p.ToWpfPoint3D()).ToList();
		}

		public Color FrontColor { get; }

		public Color BackColor { get; }

		public IReadOnlyList<Point3D> Points { get; }

		private static Color GetColor(System.Drawing.Color color)
			=> Color.FromArgb(color.A, color.R, color.G, color.B);
	}
}
