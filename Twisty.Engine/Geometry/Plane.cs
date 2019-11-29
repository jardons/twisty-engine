namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Formula representing a formula describing a plane.
	/// A plane is the collection of points for which the formula 'ax + by + cz + d = 0' is true.
	/// Note that multiple equations can describe the same plane.
	/// </summary>
	public class Plane
	{
		/// <summary>
		/// Create a new plane based on his normal and a random point that is part of the plane.
		/// </summary>
		/// <param name="normal">Vector of the normal to the plane. The normal is a perpendicular vector to the plane.</param>
		/// <param name="point">Point included in the plane and not included in the normal.</param>
		public Plane(CartesianCoordinate normal, CartesianCoordinate point)
		{
			this.Normal = normal;

			// As : 0 = ax + by + cz + d
			//      -d = ax + by + cz
			//      d = -(ax + by + cz)
			this.D = -(normal.X * point.X + normal.Y * point.Y + normal.Z * point.Z);
		}

		/// <summary>
		/// Gets the normal vector used to define this plane.
		/// </summary>
		public CartesianCoordinate Normal { get; }

		/// <summary>
		/// Gets the A factor of the formula 'ax + by + cz + d = 0' used to define a plane.
		/// </summary>
		public double A => this.Normal.X;

		/// <summary>
		/// Gets the B factor of the formula 'ax + by + cz + d = 0' used to define a plane.
		/// </summary>
		public double B => this.Normal.Y;

		/// <summary>
		/// Gets the C factor of the formula 'ax + by + cz + d = 0' used to define a plane.
		/// </summary>
		public double C => this.Normal.Z;

		/// <summary>
		/// Gets the D factor of the formula 'ax + by + cz + d = 0' used to define a plane.
		/// </summary>
		public double D { get; }

		/// <summary>
		/// Indicate if a specific point is on the plane or not.
		/// </summary>
		/// <param name="point">Point for which appartenance to the plane will be checked.</param>
		/// <returns>A boolean indicating whether the point is ont the plane or not.</returns>
		/// <remarks>Formula : ax + by + cz + d = 0</remarks>
		public bool IsOnPlane(CartesianCoordinate point) => (this.A * point.X + this.B * point.Y + this.C * point.Z + this.D).IsZero();

		/// <summary>
		/// Gets the intersection point coordinate between the current plane and the speicifed line.
		/// </summary>
		/// <param name="l">Line that should cross the plane at the seeked intersection point.</param>
		/// <returns>The cartesian coordinates of the intersection point between the line and the Plane.</returns>
		/// <remarks>
		/// Formula :
		/// 
		/// A(x1 + at) + B(y1 + bt) + C(z1 + ct) + D = 0
		/// 
		/// Then giving t :
		/// 
		///      -(Ax1 + By1 + Cz1 + D)
		/// t = ------------------------
		///           Aa + Bb + Cc
		/// 
		/// Coordinates are then :
		/// 
		///           a(Ax1 + By1 + Cz1 + D)             b(Ax1 + By1 + Cz1 + D)             c(Ax1 + By1 + Cz1 + D)
		/// x = x1 - ------------------------  y = y1 - ------------------------  z = z1 - ------------------------
		///                Aa + Bb + Cc                       Aa + Bb + Cc                       Aa + Bb + Cc
		///       
		/// </remarks>
		public CartesianCoordinate GetIntersection(ParametricLine l)
		{
			double planePart = this.A * l.X + this.B * l.Y + this.C * l.Z + this.D;
			double divisor = this.A * l.A + this.B * l.B + this.C * l.C;

			return new CartesianCoordinate(
					l.X - l.A * planePart / divisor,
					l.Y - l.B * planePart / divisor,
					l.Z - l.C * planePart / divisor
				);
		}
	}
}
