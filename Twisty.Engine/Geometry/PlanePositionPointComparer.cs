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
	public class PlanePositionPointComparer : IComparer<Cartesian3dCoordinate>, IComparer<IPositionnedBySphericalVector>
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
			Cartesian2dCoordinate x2 = m_Converter.ConvertTo2d(x);
			Cartesian2dCoordinate y2 = m_Converter.ConvertTo2d(y);

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
		public int Compare(IPositionnedBySphericalVector x, IPositionnedBySphericalVector y)
		{
			return this.Compare(CoordinateConverter.ConvertToCartesian(x.Position), CoordinateConverter.ConvertToCartesian(y.Position));
		}

		#endregion IComparer<IPositionnedBySphericalVector> Members
	}
}
