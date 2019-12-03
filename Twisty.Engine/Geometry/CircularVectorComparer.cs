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
	/// Comparer allowing to compare vector based on their angular distance relative to an initial vector.
	/// </summary>
	public class CircularVectorComparer : IComparer<SphericalVector>, IComparer<CartesianCoordinate>, IComparer<IPositionnedBySphericalVector>
	{
		private CartesianCoordinate m_StartingVectorAxis;

		/// <summary>
		/// Create a new CircularVectorComparer using the position of an object as starting vector.
		/// </summary>
		/// <param name="obj">Object from with the starting position will be used.</param>
		public CircularVectorComparer(IPositionnedBySphericalVector obj)
			: this(CoordinateConverter.ConvertToCartesian(obj.Position))
		{ }

		/// <summary>
		/// Create a new CircularVectorComparer using a starting vector.
		/// </summary>
		/// <param name="startingVector">Spherical Cordinates of the starting vector used for the comparisons.</param>
		public CircularVectorComparer(SphericalVector startingVector)
			: this(CoordinateConverter.ConvertToCartesian(startingVector))
		{ }

		/// <summary>
		/// Create a new CircularVectorComparer using a starting vector.
		/// </summary>
		/// <param name="startingVector">Cartesians Cordinates of the starting vector used for the comparisons.</param>
		public CircularVectorComparer(CartesianCoordinate startingVector)
		{
			m_StartingVectorAxis = startingVector;
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
			double thetaX = x.GetThetaTo(m_StartingVectorAxis);
			double thetaY = y.GetThetaTo(m_StartingVectorAxis);

			if (thetaX.IsEqualTo(thetaY))
				return 0;

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
	}
}