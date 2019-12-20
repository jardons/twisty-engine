using System;

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
		/// Create a new Plane from the coordiante in a string format.
		/// </summary>
		/// <param name="coordinates">Coordinates in the string format "(A B C D)".</param>
		public Plane(string coordinates)
		{
			try
			{
				double[] parsed = CoordinateConverter.ParseCoordinates(coordinates);
				if (parsed.Length != 4)
					throw new ArgumentException("The provided coordinates are not in the expected format '(A B C D)' and does not contains 4 values.", nameof(coordinates));

				this.Normal = new Cartesian3dCoordinate(parsed[0], parsed[1], parsed[2]);
				this.D = parsed[3];
			}
			catch (FormatException e)
			{
				throw new ArgumentException("The provided coordinates are not in the expected format '(A B C D)' and cannot be parsed.", nameof(coordinates), e);
			}
		}

		/// <summary>
		/// Create a new Plane.
		/// </summary>
		/// <param name="a">A factor of the formula 'ax + by + cz + d = 0' used to define a plane.</param>
		/// <param name="b">B factor of the formula 'ax + by + cz + d = 0' used to define a plane.</param>
		/// <param name="c">C factor of the formula 'ax + by + cz + d = 0' used to define a plane.</param>
		/// <param name="d">D factor of the formula 'ax + by + cz + d = 0' used to define a plane.</param>
		public Plane(double a, double b, double c, double d)
		{
			this.Normal = new Cartesian3dCoordinate(a, b, c);
			this.D = d;
		}

		/// <summary>
		/// Create a new plane based on his normal and a random point that is part of the plane.
		/// </summary>
		/// <param name="normal">Vector of the normal to the plane. The normal is a perpendicular vector to the plane.</param>
		/// <param name="point">Point included in the plane and not included in the normal.</param>
		public Plane(Cartesian3dCoordinate normal, Cartesian3dCoordinate point)
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
		public Cartesian3dCoordinate Normal { get; }

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
		public bool IsOnPlane(Cartesian3dCoordinate point) => (this.A * point.X + this.B * point.Y + this.C * point.Z + this.D).IsZero();

		/// <summary>
		/// Gets the intersection point coordinate between the current plane and another specified plane.
		/// </summary>
		/// <param name="p">Plane that should cross the plane on the seeked intersection line.</param>
		/// <returns>The parametric line at the intersection between the 2 planes.</returns>
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
		public ParametricLine GetIntersection(Plane p)
		{
			//if (l.IsParallelTo(this))
			//throw new InvalidOperationException("Cannot Get Intersection between a Plane and a parallel line.");

			// Orientation of the intersection between the plane is a vector perpendicular to the normals of both planes.
			Cartesian3dCoordinate orientation = this.Normal.CrossProduct(p.Normal);

			// Base on the plane equation, we need to adapt to use the correct resolution to avoid usses coming from potential divide by 0.0.
			Cartesian3dCoordinate point;
			if (TryGetPointAtIntersectionOnZ(p, out Cartesian3dCoordinate rZ))
				point = rZ;
			else if (TryGetPointAtIntersectionOnY(p, out Cartesian3dCoordinate rY))
				point = rY;
			else if (TryGetPointAtIntersectionOnX(p, out Cartesian3dCoordinate rX))
				point = rX;
			else
				throw new NotImplementedException("The method was not able to find a way to get the intersection between the 2 Plane with the currently implemented formula.");

			return new ParametricLine(point, orientation);
		}

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
		public Cartesian3dCoordinate GetIntersection(ParametricLine l)
		{
			if (l.IsParallelTo(this))
				throw new InvalidOperationException("Cannot Get Intersection between a Plane and a parallel line.");

			double divisor = GetSumOfAbcProduct(l);
			double planePart = (this.A * l.X + this.B * l.Y + this.C * l.Z + this.D) / divisor;

			return new Cartesian3dCoordinate(
					l.X - l.A * planePart,
					l.Y - l.B * planePart,
					l.Z - l.C * planePart
				);
		}

		/// <summary>
		/// Gets the intersection point coordinate between the current plane and the specified Vector.
		/// </summary>
		/// <param name="v">Vector that should cross the plane at the seeked intersection point.</param>
		/// <returns>The cartesian coordinates of the intersection point between the Vector and the Plane.</returns>
		/// <remarks>
		/// This is a simplified version of the formula from the ParametricLine formula.
		/// All part of the formula known as been equals to 0 are not calculated.
		/// </remarks>
		public Cartesian3dCoordinate GetIntersection(Cartesian3dCoordinate v)
		{
			double divisor = GetSumOfAbcProduct(v);
			if (divisor.IsZero())
				throw new InvalidOperationException("Cannot Get Intersection between a Plane and a parallel vector.");

			double planePart = this.D / divisor;

			return new Cartesian3dCoordinate(
					- v.X * planePart,
					- v.Y * planePart,
					- v.Z * planePart
				);
		}

		#region Private Members

		/// <summary>
		/// Gets the sum of products of the A, B and C factor of the plane and parametric line formula.
		/// </summary>
		/// <param name="l">Parametric line with which we calculate the product.</param>
		/// <returns>Result of the sum of the product of the A, B and C factor of the plane and parametric line formula.</returns>
		private double GetSumOfAbcProduct(ParametricLine l) => this.A * l.A + this.B * l.B + this.C * l.C;

		/// <summary>
		/// Gets the sum of products of the A, B and C factor of the plane and parametric line formula.
		/// </summary>
		/// <param name="cc">Parametric line with which we calculate the product.</param>
		/// <returns>Result of the sum of the product of the A, B and C factor of the plane and parametric line formula.</returns>
		private double GetSumOfAbcProduct(Cartesian3dCoordinate cc) => this.A * cc.X + this.B * cc.Y + this.C * cc.Z;

		/// <summary>
		/// Try to calculate a point at the intersection of 2 plane using linear combination.
		/// As we only have 2 plane to find a point, the Z value is predefined to 0.0.
		/// </summary>
		/// <param name="p">Second Plane with which we search the intersection point.</param>
		/// <param name="cc">Calculated coordinate of one of the intersection point between the 2 Planes in case of success.</param>
		/// <returns>A boolean indicating whther this function was able to evaluate the expected point or not.</returns>
		private bool TryGetPointAtIntersectionOnZ(Plane p, out Cartesian3dCoordinate cc)
		{
			// Avoid to divide by 0.0 at the end. (see last formula)
			if (this.B.IsZero())
			{
				cc = Cartesian3dCoordinate.Zero;
				return false;
			}

			// We have 2 formula with 3 variables :
			// A1 X + B1 Y + C1 Z + D1 = 0
			// A2 X + B2 Y + C2 Z + D2 = 0
			//
			// We will limit equations to 2 variables by predefining Z to 0.0 :
			// A1 X + B1 Y = -D1
			// A2 X + B2 Y = -D2
			//
			// We will then use linear combination between the 2 formulas :
			// A1 X B2 + B1 Y B2 = -D1 B2
			// A2 X B1 + B2 Y B1 = -D2 B1
			//
			// B1 Y B2 = -D1 B2 - A1 X B2
			// B2 Y B1 = -D2 B1 - A2 X B1
			//
			// This give us :
			//
			// A2 X B1 - A1 X B2 = D1 B2 - D2 B1
			//
			// (A2 B1 - A1 B2) X = D1 B2 - D2 B1
			//
			//      D1 B2 - D2 B1
			// X = ---------------
			//      A2 B1 - A1 B2
			double divisor = p.A * this.B - this.A * p.B;
			if (divisor.IsZero())
			{
				cc = Cartesian3dCoordinate.Zero;
				return false;
			}

			double x = (this.D * p.B - p.D * this.B) / divisor;

			// With this X calculated and Z to 0.0, we can then resolve one of the equations for Y :
			//
			//        A1 X + D1
			// Y = - -----------
			//           B1
			cc = new Cartesian3dCoordinate(
				x,
				-(this.A * x + this.D) / this.B,
				0.0
			);

			return true;
		}

		/// <summary>
		/// Try to calculate a point at the intersection of 2 plane using linear combination.
		/// As we only have 2 plane to find a point, the Y value is predefined to 0.0.
		/// </summary>
		/// <param name="p">Second Plane with which we search the intersection point.</param>
		/// <param name="cc">Calculated coordinate of one of the intersection point between the 2 Planes in case of success.</param>
		/// <returns>A boolean indicating whther this function was able to evaluate the expected point or not.</returns>
		private bool TryGetPointAtIntersectionOnY(Plane p, out Cartesian3dCoordinate cc)
		{
			// Avoid to divide by 0.0 at the end. (see last formula)
			if (this.C.IsZero())
			{
				cc = Cartesian3dCoordinate.Zero;
				return false;
			}

			// We have 2 formula with 3 variables :
			// A1 X + B1 Y + C1 Z + D1 = 0
			// A2 X + B2 Y + C2 Z + D2 = 0
			//
			// We will limit equations to 2 variables by predefining Y to 0.0 :
			// A1 X + C1 Z = -D1
			// A2 X + C2 Z = -D2
			//
			// We will then use linear combination between the 2 formulas :
			// A1 X C2 + C1 Z C2 = -D1 C2
			// A2 X C1 + C2 Z C1 = -D2 C1
			//
			// C1 Z C2 = -D1 C2 - A1 X C2
			// C2 Z C1 = -D2 C1 - A2 X C1
			//
			// This give us :
			//
			// A2 X C1 - A1 X C2 = D1 C2 - D2 C1
			//
			// (A2 C1 - A1 C2) X = D1 C2 - D2 C1
			//
			//      D1 C2 - D2 C1
			// X = ---------------
			//      A2 C1 - A1 C2
			double divisor = p.A * this.C - this.A * p.C;
			if (divisor.IsZero())
			{
				cc = Cartesian3dCoordinate.Zero;
				return false;
			}

			double x = (this.D * p.C - p.D * this.C) / divisor;

			// With this X calculated and Y to 0.0, we can then resolve one of the equations for Z :
			//
			//        A1 X + D1
			// Z = - -----------
			//           C1
			cc = new Cartesian3dCoordinate(
				x,
				0.0,
				-(this.A * x + this.D) / this.C
			);

			return true;
		}

		/// <summary>
		/// Try to calculate a point at the intersection of 2 plane using linear combination.
		/// As we only have 2 plane to find a point, the X value is predefined to 0.0.
		/// </summary>
		/// <param name="p">Second Plane with which we search the intersection point.</param>
		/// <returns>Coordinate of one of the intersection point between the 2 Planes.</returns>
		private bool TryGetPointAtIntersectionOnX(Plane p, out Cartesian3dCoordinate cc)
		{
			// Avoid to divide by 0.0 at the end. (see last formula)
			if (this.C.IsZero())
			{
				cc = Cartesian3dCoordinate.Zero;
				return false;
			}

			// We have 2 formula with 3 variables :
			// A1 X + B1 Y + C1 Z + D1 = 0
			// A2 X + B2 Y + C2 Z + D2 = 0
			//
			// We will limit equations to 2 variables by predefining X to 0.0 :
			// B1 Y + C1 Z = -D1
			// B2 Y + C2 Z = -D2
			//
			// We will then use linear combination between the 2 formulas :
			// B1 Y C2 + C1 Z C2 = -D1 C2
			// B2 Y C1 + C2 Z C1 = -D2 C1
			//
			// C1 Z C2 = -D1 C2 - B1 Y C2
			// C2 Z C1 = -D2 C1 - B2 Y C1
			//
			// This give us :
			//
			// B2 Y C1 - B1 Y C2 = D1 C2 - D2 C1
			//
			// (B2 C1 - B1 C2) Y = D1 C2 - D2 C1
			//
			//      D1 C2 - D2 C1
			// Y = ---------------
			//      B2 C1 - B1 C2
			double divisor = p.B * this.C - this.B * p.C;
			if (divisor.IsZero())
			{
				cc = Cartesian3dCoordinate.Zero;
				return false;
			}

			double y = (this.D * p.C - p.D * this.C) / divisor;

			// With this Y calculated and X to 0.0, we can then resolve one of the equations for Z :
			//
			//        B1 Y + D1
			// Z = - -----------
			//           C1
			cc = new Cartesian3dCoordinate(
				0.0,
				y,
				-(this.B * y + this.D) / this.C
			);

			return true;
		}

		#endregion Private Members
	}
}