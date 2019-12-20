namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Define the coordinate of a Point or Vector using the Homogeneous coordinates.
	/// </summary>
	public struct HomogeneousCoordinate
	{
		/// <summary>
		/// Create a new Cartesian3dCoordinate object.
		/// </summary>
		/// <param name="x">Coordinates on the X axis.</param>
		/// <param name="y">Coordinates on the Y axis.</param>
		/// <param name="z">Coordinates on the Z axis.</param>
		/// <param name="w">Coordinates  in the projective space.</param>
		public HomogeneousCoordinate(double x, double y, double z, double w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		#region Public Properties

		/// <summary>
		/// Gets the cartesian coordinate on the X axis.
		/// </summary>
		public double X { get; }

		/// <summary>
		/// Gets the cartesian coordinate on the Y axis.
		/// </summary>
		public double Y { get; }

		/// <summary>
		/// Gets the cartesian coordinate on the Z axis.
		/// </summary>
		public double Z { get; }

		/// <summary>
		/// Gets the coordinates in the projective space.
		/// </summary>
		public double W { get; }

		#endregion Public Properties
	}
}
