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
			this.FrontColor = GetColor(p.ColorId);
			this.BackColor = GetColor(p.ColorId);
			this.Points = p.Points.Select(p => p.ToWpfPoint3D()).ToList();
		}

		public Color FrontColor { get; }

		public Color BackColor { get; }

		public IReadOnlyList<Point3D> Points { get; }

		private static Color GetColor(string faceId = null)
		{
			return faceId switch
			{
				"F" => Color.FromRgb(0, 0, 255),
				"L" => Color.FromRgb(255, 105, 0),
				"R" => Color.FromRgb(255, 0, 0),
				"D" => Color.FromRgb(255, 255, 255),
				"B" => Color.FromRgb(0, 250, 0),
				"U" => Color.FromRgb(255, 255, 0),
				// Return black when not able to match a color.
				_ => Color.FromRgb(0, 0, 0),
			};
		}
	}
}
