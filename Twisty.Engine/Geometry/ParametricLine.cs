using System;

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

				this.Point = new CartesianCoordinate(parsed[0], parsed[1], parsed[2]);
				this.Vector = new CartesianCoordinate(parsed[3], parsed[4], parsed[5]);
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
			this.Point = new CartesianCoordinate(x, y, z);
			this.Vector = new CartesianCoordinate(a, b, c);
		}

		/// <summary>
		/// Create a ParametricLine starting at the initial coordinate and following a provided Vector.
		/// </summary>
		/// <param name="v">Vector providing the direction of the line.</param>
		public ParametricLine(CartesianCoordinate v)
		{
			this.Point = CartesianCoordinate.Zero;
			this.Vector = v;
		}

		/// <summary>
		/// Create a ParametricLine starting from a specific point and following a provided Vector.
		/// </summary>
		/// <param name="p">Initial point of the line.</param>
		/// <param name="v">Vector providing the direction of the line.</param
		public ParametricLine(CartesianCoordinate p, CartesianCoordinate v)
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
		public CartesianCoordinate Point { get; }
		/// <summary>
		/// Gets the directional vector of the line.
		/// </summary>
		public CartesianCoordinate Vector { get; }

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Evaluate if the current line is parallel to a given plane.
		/// </summary>
		/// <param name="p">Plane to which the orientation of the Line will be compared.</param>
		/// <returns>A boolean indicating whether the Line and the Plane are parallel or not.</returns>
		/// <remarks>
		/// A line is parallel to a Plan when the this calculation is true :
		/// aA + bB + cC = 0
		/// </remarks>
		public bool IsParallelTo(Plane p)
		{
			return (this.A * p.A + this.B * p.B + this.C * p.C).IsZero();
		}

		/// <summary>
		/// Create a ParametricLine between 2 lines.
		/// </summary>
		/// <param name="p1">Starting point of the line.</param>
		/// <param name="p2">Ending point of the line.</param>
		/// <returns>The Parametric line linking the provided points.</returns>
		public static ParametricLine FromTwoPoints(CartesianCoordinate p1, CartesianCoordinate p2)
		{
			return new ParametricLine(p1, new CartesianCoordinate(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z));
		}

		#endregion Public methods
	}
}
