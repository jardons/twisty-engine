using System;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Converter class allowing to project 3D coordinates on a 2D Plane.
	/// </summary>
	public class CartesianCoordinatesConverter
	{
		private readonly Func<CartesianCoordinate, Cartesian2dCoordinate> m_From2dTo3d;

		/// <summary>
		/// Create a new CartesianCoordinatesConverter for a specific Plane.
		/// </summary>
		/// <param name="p">PLane on which all converted coordinates will be projected.</param>
		public CartesianCoordinatesConverter(Plane p)
		{
			this.Plane = p;
			m_From2dTo3d = Get2dConvertion(p);
		}

		/// <summary>
		/// Gets the Plane on which the points are projected.
		/// </summary>
		public Plane Plane { get; }

		/// <summary>
		/// COnvert the provided 3D coordiantes to their projected 2D counterpart on the Plane used to create this CartesianCoordinatesConverter.
		/// </summary>
		/// <param name="c3d">2D coordinates to project on the Pland and converts in 2D.</param>
		/// <returns>2D coordinates projectes on the Plane.</returns>
		public Cartesian2dCoordinate ConvertTo2d(CartesianCoordinate c3d) => m_From2dTo3d(c3d);

		#region Private Members

		/// <summary>
		/// Gets the convertion function the most adapted to the provided Plane.
		/// </summary>
		/// <param name="p">PLane used to project the points in 2D.</param>
		/// <returns>A function that can be used to convert 3D coordinates to 2D.</returns>
		private Func<CartesianCoordinate, Cartesian2dCoordinate> Get2dConvertion(Plane p)
		{
			if (p.Normal.IsOnX)
			{
				if (p.Normal.X > 0.0)
					return GetFromBack;

				return GetFromFront;
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
		/// <returns>THe converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromTop(CartesianCoordinate cc) => new Cartesian2dCoordinate(cc.X, cc.Y);

		/// <summary>
		/// Gets the 2D coordinates as seen from the bottom.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>THe converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromBottom(CartesianCoordinate cc) => new Cartesian2dCoordinate(-cc.X, cc.Y);

		/// <summary>
		/// Gets the 2D coordinates as seen from the front.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>THe converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromFront(CartesianCoordinate cc) => new Cartesian2dCoordinate(cc.Y, cc.Z);

		/// <summary>
		/// Gets the 2D coordinates as seen from the back.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>THe converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromBack(CartesianCoordinate cc) => new Cartesian2dCoordinate(-cc.Y, cc.Z);

		/// <summary>
		/// Gets the 2D coordinates as seen from the right.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>THe converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromRight(CartesianCoordinate cc) => new Cartesian2dCoordinate(cc.X, cc.Z);

		/// <summary>
		/// Gets the 2D coordinates as seen from the left.
		/// </summary>
		/// <param name="cc">3D coordinates to convert.</param>
		/// <returns>THe converted 2D coordinates.</returns>
		private Cartesian2dCoordinate GetFromLeft(CartesianCoordinate cc) => new Cartesian2dCoordinate(-cc.X, cc.Z);

		#endregion Private Members
	}
}