namespace Twisty.Engine.Geometry;

///<summary>
/// This Comparer allow to sort the point in a plane by reading them in the order Z, Y, X.
///</summary>
public class PositionPointComparer : IComparer<Cartesian2dCoordinate>, IComparer<Cartesian3dCoordinate>, IComparer<IPositionnedByCartesian3dVector>
{
	#region IComparer<Cartesian2dCoordinate> Members

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
	public int Compare(Cartesian2dCoordinate x, Cartesian2dCoordinate y)
	{
		if (!x.Y.IsEqualTo(y.Y))
			return x.Y > y.Y ? -1 : 1;

		// Same line
		if (x.X.IsEqualTo(y.X))
			return 0;

		return x.X < y.X ? -1 : 1;
	}

	#endregion IComparer<Cartesian2dCoordinate> Members

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
		if (!x.Z.IsEqualTo(y.Z))
			return x.Z > y.Z ? -1 : 1;

		// Same slice.
		if (!x.Y.IsEqualTo(y.Y))
			return x.Y > y.Y ? -1 : 1;

		// Same line
		if (x.X.IsEqualTo(y.X))
			return 0;

		return x.X < y.X ? -1 : 1;
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
