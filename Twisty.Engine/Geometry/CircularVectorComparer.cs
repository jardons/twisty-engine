using System;
using System.Collections.Generic;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Interface used to identify objects positionned using a SphericalVector relative to a central point.
	/// </summary>
	public interface IPositionnedBySphericalVector
	{
		/// <summary>
		/// Position is stored using the direction relative to the a center.
		/// </summary>
		SphericalVector Position { get; }
	}

	/// <summary>
	/// Comparer allowing to compare vector based on their angular distance to the X axis in a Plane.
	/// The sorting direction is counter-clockwise.
	/// </summary>
	public class CircularVectorComparer : IComparer<SphericalVector>, IComparer<CartesianCoordinate>, IComparer<IPositionnedBySphericalVector>
	{
		private CartesianCoordinatesConverter m_Converter;

		/// <summary>
		/// Create a new CircularVectorComparer using a Plane on which we will projects the points to sort..
		/// </summary>
		/// <param name="p">PLane used to project the points prior to sort them.</param>
		public CircularVectorComparer(Plane p)
		{
			m_Converter = new CartesianCoordinatesConverter(p);
		}

		#region IComparer<SphericalVector> Members

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
		public int Compare(SphericalVector x, SphericalVector y)
		{
			return this.Compare(CoordinateConverter.ConvertToCartesian(x), CoordinateConverter.ConvertToCartesian(y));
		}

		#endregion IComparer<SphericalVector> Members

		#region IComparer<CartesianCoordinate> Members

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
		public int Compare(CartesianCoordinate x, CartesianCoordinate y)
		{
			// Check perfect equality first to avoid further calculations.
			if (x.X.IsEqualTo(y.X) && x.Y.IsEqualTo(y.Y) && x.Z.IsEqualTo(y.Z))
				return 0;

			Cartesian2dCoordinate x2 = m_Converter.ConvertTo2d(x);
			Cartesian2dCoordinate y2 = m_Converter.ConvertTo2d(y);

			// Check 2D perfect equality first to avoid further calculations.
			if (x2.X.IsEqualTo(y2.X) && x2.Y.IsEqualTo(y2.Y))
				return 0;

			// Get Theta value related to the X vector, if one in the starting one we don't need to compare further.
			double thetaX = x2.ThetaToX;
			if (thetaX.IsZero())
				return -1;

			double thetaY = y2.ThetaToX;
			if (thetaY.IsZero())
				return 1;

			thetaX = AlignThetaOnRotationDirection(x2, thetaX);
			thetaY = AlignThetaOnRotationDirection(y2, thetaY);

			return thetaX > thetaY ? 1 : -1;
		}

		#endregion IComparer<CartesianCoordinate> Members

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
			return this.Compare(x.Position, y.Position);
		}

		#endregion IComparer<IPositionnedBySphericalVector> Members

		#region Private Members

		/// <summary>
		/// Align the theta of calculated value based on their direction, negative value will be accessible trough bigger angle than positive one.
		/// </summary>
		/// <param name="cc">Coordinates used to realign the theta angle.</param>
		/// <param name="theta">Angle in radians to realign.</param>
		/// <returns>Realigned angles value in radians.</returns>
		private double AlignThetaOnRotationDirection(Cartesian2dCoordinate cc, double theta)
		{
			if (cc.Y > 0.0)
				return theta;

			return Math.PI * 2.0 - theta;
		}

		#endregion Private Members
	}
}