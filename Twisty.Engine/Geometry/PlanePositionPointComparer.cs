using System;
using System.Collections.Generic;

namespace Twisty.Engine.Geometry
{
	///<summary>
	/// This Coparer allow to sort the point in a plane by reading them line by line.
	///</summary>
	/// <remarks>
	/// Sorting map :
	/// 
	///        +y
	///         |
	///     1   |   2
	///        3|
	/// -x -----*----- +x
	///      4  |
	///     5   |   6
	///    7    |
	///        -y
	/// </remarks>
	public class PlanePositionPointComparer : IComparer<Cartesian3dCoordinate>, IComparer<IPositionnedByCartesian3dVector>
	{
		private CartesianCoordinatesConverter m_Converter;

		public PlanePositionPointComparer(Plane p)
		{
			m_Converter = new CartesianCoordinatesConverter(p);
		}

		#region IComparer<Cartesian3dCoordinate> Members

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// A signed integer that indicates the relative values of x and y:
		/// - If less than 0, x is less than y.
		/// - If 0, x equals y.
		/// - If greater than 0, x is greater than y.
		/// </returns>
		public int Compare(Cartesian3dCoordinate x, Cartesian3dCoordinate y)
		{
			// Calculate intersection with plane to have all point on same plane.
			Cartesian3dCoordinate intersectX = m_Converter.Plane.GetIntersection(x);
			Cartesian3dCoordinate intersectY = m_Converter.Plane.GetIntersection(y);

			// Convert in a 2D referential to facilitate the comparison.
			Cartesian2dCoordinate x2 = m_Converter.ConvertTo2d(intersectX);
			Cartesian2dCoordinate y2 = m_Converter.ConvertTo2d(intersectY);

			if (x2.Y.IsEqualTo(y2.Y))
			{
				// Same line
				if (x2.X.IsEqualTo(y2.X))
					return 0;

				return x2.X < y2.X ? -1 : 1;
			}

			return x2.Y > y2.Y ? -1 : 1;
		}

		#endregion IComparer<Cartesian3dCoordinate> Members

		#region IComparer<IPositionnedBySphericalVector> Members

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// A signed integer that indicates the relative values of x and y:
		/// - If less than 0, x is less than y.
		/// - If 0, x equals y.
		/// - If greater than 0, x is greater than y.
		/// </returns>
		public int Compare(IPositionnedByCartesian3dVector x, IPositionnedByCartesian3dVector y) => this.Compare(x.Position, y.Position);

		#endregion IComparer<IPositionnedBySphericalVector> Members
	}
}
