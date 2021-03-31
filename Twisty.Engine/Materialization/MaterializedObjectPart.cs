using System;
using System.Collections.Generic;
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
		/// <param name="colorId">ID of the color used for this object part.</param>
		/// <param name="parts">Ordered collection of vertice delimiting this part.</param>
		internal MaterializedObjectPart(string colorId, IEnumerable<Cartesian3dCoordinate> points)
		{
			this.Points = points;
			this.ColorId = colorId;
		}

		/// <summary>
		/// Gets the id identifying the color of this part.
		/// </summary>
		public string ColorId { get; }

		/// <summary>
		/// Gets the ordered list of vertices defining this part of the object.
		/// </summary>
		public IEnumerable<Cartesian3dCoordinate> Points { get; }
	}
}
