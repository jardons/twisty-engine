using System;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Converter class allowing to project 3D coordinates on a 2D Plane.
	/// </summary>
	public class CartesianCoordinatesConverter
	{
		private readonly Func<Cartesian3dCoordinate, Cartesian2dCoordinate> m_From3dTo2d;

		/// <summary>
		/// Create a new Cartesian3dCoordinatesConverter for a specific Plane.
		/// </summary>
		/// <param name="p">Plane on which all converted coordinates will be projected.</param>
		public CartesianCoordinatesConverter(Plane p)
		{
			this.Plane = p;
			m_From3dTo2d = Get2dConvertion(p);
		}

		/// <summary>
		/// Gets the Plane on which the points are projected.
		/// </summary>
		public Plane Plane { get; }

		/// <summary>
		/// COnvert the provided 3D coordiantes to their projected 2D counterpart on the Plane used to create this Cartesian3dCoordinatesConverter.
		/// </summary>
		/// <param name="c3d">2D coordinates to project on the Pland and converts in 2D.</param>
		/// <returns>2D coordinates projectes on the Plane.</returns>
		public Cartesian2dCoordinate ConvertTo2d(Cartesian3dCoordinate c3d) => m_From3dTo2d(c3d);

		#region Private Members

		/// <summary>
		/// Gets the convertion function the most adapted to the provided Plane.
		/// </summary>
		/// <param name="p">PLane used to project the points in 2D.</param>
		/// <returns>A function that can be used to convert 3D coordinates to 2D.</returns>
		private Func<Cartesian3dCoordinate, Cartesian2dCoordinate> Get2dConvertion(Plane p)
		{
			if (p.Normal.IsOnX)
			{
				if (p.Normal.X > 0.0)
					return GetFromFront;

				return GetFromBack;
			}

			if (p.Normal.IsOnY)
			{
				if (p.Normal.Y > 0.0)
					return GetFromRight;

				return GetFromLeft;
			}

			if (p.Normal.IsOnZ)
			{
				if (p.Normal.Z > 0.0)
					return GetFromTop;

				return GetFromBottom;
			}

			throw new NotImplementedException("Conversion to 2D on this plane doesn't exist yet.");
		}

		/// <summary>
		/// Gets the 2D coordinates as seen from the top.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>The converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromTop(Cartesian3dCoordinate cc) => new Cartesian2dCoordinate(cc.Y, -cc.X);

		/// <summary>
		/// Gets the 2D coordinates as seen from the bottom.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>The converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromBottom(Cartesian3dCoordinate cc) => new Cartesian2dCoordinate(cc.Y, cc.X);

		/// <summary>
		/// Gets the 2D coordinates as seen from the front.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>The converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromFront(Cartesian3dCoordinate cc) => new Cartesian2dCoordinate(cc.Y, cc.Z);

		/// <summary>
		/// Gets the 2D coordinates as seen from the back.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>The converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromBack(Cartesian3dCoordinate cc) => new Cartesian2dCoordinate(-cc.Y, cc.Z);

		/// <summary>
		/// Gets the 2D coordinates as seen from the right.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>The converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromRight(Cartesian3dCoordinate cc) => new Cartesian2dCoordinate(-cc.X, cc.Z);

		/// <summary>
		/// Gets the 2D coordinates as seen from the left.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>The converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromLeft(Cartesian3dCoordinate cc) => new Cartesian2dCoordinate(cc.X, cc.Z);

		#endregion Private Members
	}
}