using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Immutable class providing cartesian coordinates representation of a point or vector in a 2 dimensions plane.
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
		public static readonly Cartesian2dCoordinate Zero = new(0.0, 0.0);

		#region ctor(s)

		/// <summary>
		/// Create a new Cartesian3dCoordinate from a coordinates string on the format "(X Y)".
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
		/// Create a new Cartesian3dCoordinate object.
		/// </summary>
		/// <param name="x">Coordinates on the X axis.</param>
		/// <param name="y">Coordinates on the Y axis.</param>
		public Cartesian2dCoordinate(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		#endregion ctor(s)

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

		/// <summary>
		/// Gets the magnitude of this vector, also noted as ||V||.
		/// </summary>
		/// <remarks>
		/// Formula :
		///          ___________
		/// ||V|| = V Xv² * Yv² '
		/// </remarks>
		public double Magnitude => Math.Sqrt((this.X * this.X) + (this.Y * this.Y));

		/// <summary>
		/// Gets the angle in radians between the vector and the X axis.
		/// </summary>
		public double ThetaToX => Math.Acos(this.X / this.Magnitude);

		/// <summary>
		/// Gets the angle in radians between the vector and the Y axis.
		/// </summary>
		public double ThetaToY => Math.Acos(this.Y / this.Magnitude);

		#endregion Public Properties

		#region Operators

		/// <summary>
		/// Gets the result of the substraction of 2 vectors.
		/// </summary>
		/// <param name="v1">First vector from which the second one will be substracted.</param>
		/// <param name="v2">Substracted Vector.</param>
		/// <returns>Result of the subbstraction of the 2 vectors.</returns>
		public static Cartesian2dCoordinate operator -(in Cartesian2dCoordinate v1, in Cartesian2dCoordinate v2)
		{
			return new Cartesian2dCoordinate(
					v1.X - v2.X,
					v1.Y - v2.Y
				);
		}

		/// <summary>
		/// Gets the sum of 2 vectors.
		/// </summary>
		/// <param name="v1">First vector to add.</param>
		/// <param name="v2">Second vector to add.</param>
		/// <returns>Result of the sum of the 2 vectors.</returns>
		public static Cartesian2dCoordinate operator +(in Cartesian2dCoordinate v1, in Cartesian2dCoordinate v2)
		{
			return new Cartesian2dCoordinate(
					v1.X + v2.X,
					v1.Y + v2.Y
				);
		}

		#endregion Operators
	}
}
