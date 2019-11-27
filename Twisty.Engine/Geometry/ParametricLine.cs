namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Class defining a line between 2 points in his parametrics form.
	/// 
	/// The formula used to defined a parametric line is :
	/// x = x1 + a * t
	/// y = y1 + b * t
	/// z = z1 + b * t
	/// </summary>
	public class ParametricLine
	{
		public ParametricLine(double x, double y, double z, double a, double b, double c)
		{
			this.Point = new CartesianCoordinate(x, y, z);
			this.Vector = new CartesianCoordinate(a, b, c);
		}

		public ParametricLine(CartesianCoordinate v)
		{
			this.Point = CartesianCoordinate.Zero;
			this.Vector = v;
		}

		public ParametricLine(CartesianCoordinate p, CartesianCoordinate v)
		{
			this.Point = p;
			this.Vector = v;
		}

		#region Public Properties

		public double X => Point.X;
		public double Y => Point.Y;
		public double Z => Point.Z;

		public double A => Vector.X;
		public double B => Vector.Y;
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
