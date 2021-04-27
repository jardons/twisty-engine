using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Twisty.Runner.Models.Model3d;

namespace Twisty.Runner.Wpf.Model3d
{
	public class BlockPart3dObject
	{
		private Core3dSurface m_Source;

		public BlockPart3dObject(Core3dSurface part)
		{
			m_Source = part;
			this.GeometryModel = new()
			{
				Geometry = CreateMesh(part.Points),
				Material = GetBrush(part.FrontColor),
				BackMaterial = GetBrush(part.BackColor)
			};
		}

		public GeometryModel3D GeometryModel { get; }

		/// <summary>
		/// Switch the current object to an alternate color.
		/// </summary>
		/// <param name="c">Alternate collor used to display the object.</param>
		public void SetColor(Color c)
		{
			var b = GetBrush(c);
			GeometryModel.Material = b;
			GeometryModel.BackMaterial = b;
		}

		/// <summary>
		/// Reset this object color to original value.
		/// </summary>
		public void ResetColor()
		{
			GeometryModel.Material = GetBrush(m_Source.FrontColor);
			GeometryModel.BackMaterial = GetBrush(m_Source.BackColor);
		}

		/// <summary>
		/// Create the Mesh for a single block face.
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		private static MeshGeometry3D CreateMesh(IReadOnlyList<Point3D> points)
		{
			// Mesh will be created by suming triangle sharing a common vertice in the center of the surface.
			MeshGeometry3D geo = new();
			geo.Positions.Add(points[0]);
			geo.Positions.Add(points[1]);
			geo.Positions.Add(points[2]);

			geo.TriangleIndices.Add(0);
			geo.TriangleIndices.Add(1);
			geo.TriangleIndices.Add(2);

			for (int i = 3; i < points.Count; ++i)
			{
				// Update positions.
				geo.Positions.Add(points[i]);

				geo.TriangleIndices.Add(0);         // First Point
				geo.TriangleIndices.Add(i - 1);     // Previous
				geo.TriangleIndices.Add(i);         // Current
			}

			return geo;
		}

		private static DiffuseMaterial GetBrush(Color c)
			=> new() { Brush = new SolidColorBrush(c) };
	}
}
