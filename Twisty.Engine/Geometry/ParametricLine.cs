using System;
using System.Diagnostics;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Class defining a line between 2 points in his parametrics form.
	/// 
	/// The formula used to defined a parametric line is :
	/// x = x1 + a * t
	/// y = y1 + b * t
	/// z = z1 + c * t
	/// </summary>
	[DebuggerDisplay("({Point.X}, {Point.Y}, {Point.Z} # {Vector.X}, {Vector.Y}, {Vector.Z})")]
	public class ParametricLine
	{
		/// <summary>
		/// Create a new ParametricLine from a coordinates string on the format "(X Y Z A B C)".
		/// </summary>
		/// <param name="coordinates">Coordinates in the format "(X Y Z A B C)".</param>
		public ParametricLine(string coordinates)
		{
			try
			{
				double[] parsed = CoordinateConverter.ParseCoordinates(coordinates);
				if (parsed.Length != 6)
					throw new ArgumentException("The provided coordinates are not in the expected format '(X Y Z A B C)' and does not contains 3 values.", nameof(coordinates));

				this.Point = new Cartesian3dCoordinate(parsed[0], parsed[1], parsed[2]);
				this.Vector = new Cartesian3dCoordinate(parsed[3], parsed[4], parsed[5]);
			}
			catch (FormatException e)
			{
				throw new ArgumentException("The provided coordinates are not in the expected format '(X Y Z A B C)' and cannot be parsed.", nameof(coordinates), e);
			}
		}

		/// <summary>
		/// Create a new ParametricLine from all the variable of the identification formula.
		/// </summary>
		/// <param name="x">X coordinate of the initial point of the Line.</param>
		/// <param name="y">Y coordinate of the initial point of the Line.</param>
		/// <param name="z">Z coordinate of the initial point of the Line.</param>
		/// <param name="a">A factor of the Line formula.</param>
		/// <param name="b">B factor of the Line formula.</param>
		/// <param name="c">C factor of the Line formula.</param>
		public ParametricLine(double x, double y, double z, double a, double b, double c)
		{
			this.Point = new Cartesian3dCoordinate(x, y, z);
			this.Vector = new Cartesian3dCoordinate(a, b, c);
		}

		/// <summary>
		/// Create a ParametricLine starting at the initial coordinate and following a provided Vector.
		/// </summary>
		/// <param name="v">Vector providing the direction of the line.</param>
		public ParametricLine(Cartesian3dCoordinate v)
		{
			this.Point = Cartesian3dCoordinate.Zero;
			this.Vector = v;
		}

		/// <summary>
		/// Create a ParametricLine starting from a specific point and following a provided Vector.
		/// </summary>
		/// <param name="p">Initial point of the line.</param>
		/// <param name="v">Vector providing the direction of the line.</param
		public ParametricLine(Cartesian3dCoordinate p, Cartesian3dCoordinate v)
		{
			this.Point = p;
			this.Vector = v;
		}

		#region Public Properties

		/// <summary>
		/// Gets the X coordinate of the initial point of the Line.
		/// </summary>
		public double X => Point.X;

		/// <summary>
		/// Gets the Y coordinate of the initial point of the Line.
		/// </summary>
		public double Y => Point.Y;

		/// <summary>
		/// Gets the Z coordinate of the initial point of the Line.
		/// </summary>
		public double Z => Point.Z;

		/// <summary>
		/// Gets the A factor of the Line formula.
		/// </summary>
		public double A => Vector.X;

		/// <summary>
		/// Gets the B factor of the Line formula.
		/// </summary>
		public double B => Vector.Y;

		/// <summary>
		/// Gets the C factor of the Line formula.
		/// </summary>
		public double C => Vector.Z;

		/// <summary>
		/// Gets the coordinates of the starting point of the line.
		/// </summary>
		public Cartesian3dCoordinate Point { get; }
		/// <summary>
		/// Gets the directional vector of the line.
		/// </summary>
		public Cartesian3dCoordinate Vector { get; }

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Evaluate if the current line is parallel to a given plane.
		/// </summary>
		/// <param name="p">Plane to which the orientation of the Line will be compared.</param>
		/// <returns>A boolean indicating whether the Line and the Plane are parallel or not.</returns>
		/// <remarks>
		/// A line is parallel to a Plan when this calculation is true :
		/// aA + bB + cC = 0
		/// </remarks>
		public bool IsParallelTo(Plane p)
			=> ((this.A * p.A) + (this.B * p.B) + (this.C * p.C)).IsZero();

		/// <summary>
		/// Evaluate if the current line is parallel to another given line.
		/// </summary>
		/// <param name="l">Line to which the orientation of the Line will be compared.</param>
		/// <returns>A boolean indicating whether both the Lines are parallel or not.</returns>
		public bool IsParallelTo(ParametricLine l)
			=> this.Vector.IsSameVector(l.Vector) || this.Vector.IsSameVector(l.Vector.Reverse);

		/// <summary>
		/// Gets the shortest distance between a point and the line.
		/// </summary>
		/// <param name="point">Coordinates of the point that will be compared to the Line.</param>
		/// <returns>A double value indicating the distance to the Line.</returns>
		/// <remarks>
		/// Distance can be calculated by building a parallelogram and calculate it height from its surface.
		/// 
		///       p1        v1
		/// ------*===================*---------------
		///      /                |  /
		///     /                d| /
		///    /                  |/
		///   *===================*
		///  p0
		/// 
		/// Surface:
		/// S = (p1 - p0) x v1
		/// 
		/// Distance:
		/// d = || S || / || v1 ||
		/// </remarks>
		public double GetDistanceTo(Cartesian3dCoordinate point)
			=> (this.Point - point).CrossProduct(this.Vector).Magnitude / this.Vector.Magnitude;

		/// <summary>
		/// Create a ParametricLine between 2 lines.
		/// </summary>
		/// <param name="p1">Starting point of the line.</param>
		/// <param name="p2">Ending point of the line.</param>
		/// <returns>The Parametric line linking the provided points.</returns>
		public static ParametricLine FromTwoPoints(Cartesian3dCoordinate p1, Cartesian3dCoordinate p2)
		{
			return new ParametricLine(p1, new Cartesian3dCoordinate(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z));
		}

		#endregion Public methods
	}
}
