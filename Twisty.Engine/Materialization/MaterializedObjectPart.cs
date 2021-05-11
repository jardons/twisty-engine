using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Materialization
{
	/// <summary>
	/// Model object representing the vertices of one part of a RotationCore object in 3d.
	/// </summary>
	public class MaterializedObjectPart
	{
		/// <summary>
		/// Create a new MaterializedObject from the list of his internal parts.
		/// </summary>
		/// <param name="color">Color used for this object part.</param>
		/// <param name="parts">Ordered collection of vertice delimiting this part.</param>
		internal MaterializedObjectPart(Color color, IEnumerable<Cartesian3dCoordinate> points)
		{
			this.Points = points;
			this.Color = color;
		}

		/// <summary>
		/// Gets the color of this part.
		/// </summary>
		public Color Color { get; }

		/// <summary>
		/// Gets the ordered list of vertices defining this part of the object.
		/// </summary>
		public IEnumerable<Cartesian3dCoordinate> Points { get; }
	}
}
