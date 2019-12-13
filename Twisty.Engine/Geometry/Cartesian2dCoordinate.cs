using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Class providing cartesian coordinates representation of a point or vector in a 2 dimensions plane.
	/// Vector coordinates are representated by the combination of coordinates following the 2 perpendicular axis X and Y.
	/// </summary>
	/// <example>
	/// Diagram :
	/// 
	///          +y
	///           |
	///           |
	///   -x -----O----- +x
	///           |
	///           |
	///          -y
	///          
	/// </example>
	[DebuggerDisplay("({X}, {Y})")]
	public class Cartesian2dCoordinate
	{
		/// <summary>
		/// Gets the Zero point coordinates.
		/// </summary>
		public static readonly Cartesian2dCoordinate Zero = new Cartesian2dCoordinate(0.0, 0.0);

		/// <summary>
		/// Create a new CartesianCoordinate from a coordinates string on the format "(X Y)".
		/// </summary>
		/// <param name="coordinates">Coordinates in the format "(X Y)".</param>
		public Cartesian2dCoordinate(string coordinates)
		{
			try
			{
				double[] parsed = CoordinateConverter.ParseCoordinates(coordinates);
				if (parsed.Length != 2)
					throw new ArgumentException("The provided coordinates are not in the expected format '(X Y)' and does not contains 3 values.", nameof(coordinates));

				this.X = parsed[0];
				this.Y = parsed[1];
			}
			catch (FormatException e)
			{
				throw new ArgumentException("The provided coordinates are not in the expected format '(X Y)' and cannot be parsed.", nameof(coordinates), e);
			}
		}

		/// <summary>
		/// Create a new CartesianCoordinate object.
		/// </summary>
		/// <param name="x">Coordinates on the X axis.</param>
		/// <param name="y">Coordinates on the Y axis.</param>
		public Cartesian2dCoordinate(double x, double y)
		{
			this.X = x;
			this.Y = y;
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
		/// Gets a boolean indicating if whether the current coordonate is on the X axis or not.
		/// </summary>
		public bool IsOnX => Y.IsZero();

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is on the Y axis or not.
		/// </summary>
		public bool IsOnY => X.IsZero();

		#endregion Public Properties
	}
}
