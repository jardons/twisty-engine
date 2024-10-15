using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Twisty.Engine.Geometry;

/// <summary>
/// Immutable class defining a line between 2 points in his parametrics form.
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
	public ParametricLine(in Cartesian3dCoordinate v)
	{
		this.Point = Cartesian3dCoordinate.Zero;
		this.Vector = v;
	}

	/// <summary>
	/// Create a ParametricLine starting from a specific point and following a provided Vector.
	/// </summary>
	/// <param name="p">Initial point of the line.</param>
	/// <param name="v">Vector providing the direction of the line.</param
	public ParametricLine(in Cartesian3dCoordinate p, in Cartesian3dCoordinate v)
	{
		this.Point = p;
		this.Vector = v;
	}

	#region Public Properties

	/// <summary>
	/// Gets the X coordinate of the initial point of the Line.
	/// </summary>
	[JsonIgnore]
	public double X => Point.X;

	/// <summary>
	/// Gets the Y coordinate of the initial point of the Line.
	/// </summary>
	[JsonIgnore]
	public double Y => Point.Y;

	/// <summary>
	/// Gets the Z coordinate of the initial point of the Line.
	/// </summary>
	[JsonIgnore]
	public double Z => Point.Z;

	/// <summary>
	/// Gets the A factor of the Line formula.
	/// </summary>
	[JsonIgnore]
	public double A => Vector.X;

	/// <summary>
	/// Gets the B factor of the Line formula.
	/// </summary>
	[JsonIgnore]
	public double B => Vector.Y;

	/// <summary>
	/// Gets the C factor of the Line formula.
	/// </summary>
	[JsonIgnore]
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
	public bool IsParallelTo(in Plane p)
		=> ((this.A * p.A) + (this.B * p.B) + (this.C * p.C)).IsZero();

	/// <summary>
	/// Evaluate if the current line is parallel to another given line.
	/// </summary>
	/// <param name="l">Line to which the orientation of the Line will be compared.</param>
	/// <returns>A boolean indicating whether both the Lines are parallel or not.</returns>
	public bool IsParallelTo(in ParametricLine l)
		=> this.Vector.IsSameVector(l.Vector) || this.Vector.IsSameVector(l.Vector.Reverse);

	/// <summary>
	/// Gets a boolean indicating if whether the provided point belong to the line or note.
	/// </summary>
	/// <param name="point">Coordiantes of the point for which we will test if he belong to the line or not.</param>
	/// <returns>A boolean indicating if whether the provided point belong to the line or note.</returns>
	public bool Contains(in Cartesian3dCoordinate point)
	{
		// A point belong to the line when t has the same value for both of the 3 formula :
		// { x = x0 + at
		// { y = y0 + bt
		// { z = z0 + ct

		// This can lead to divide per zero, we will start per filtering those cases.
		// A value of 0.0 for A, B or C would indicate that X, Y or Z is a constant for the line.
		// We can then exclude all points with variation on thoses axes.
		double t = double.NaN;
		if (this.A.IsZero())
		{
			if (!this.X.IsEqualTo(point.X))
				return false;

			// If X axis didn't provide a t value, we can test both Y and Z axis as a single test without getting any t value.
			// This allow us to avoid to check for the division by 0.
			// ct = z - z0
			// t = (z - z0) / c = (y - y0) / b
			// (z - z0) * b = (y - y0) * c
			return ((point.Z - this.Z) * this.B).IsEqualTo((point.Y - this.Y) * this.C);
		}

		// X axis is kept as a comparison reference.
		t = (point.X - this.X) / this.A;

		// When a value is identified as t, we can compute in a faster way by avoiding :
		// * Divide and potential divide by 0
		// * Use IsZero() instead of IsEqualTo().
		// As an exemple:
		// z = z0 + ct
		// 0 = z0 + ct - z
		// This equation should be true when in the line.
		return (this.Y + (this.B * t) - point.Y).IsZero()
			&& (this.Z + (this.C * t) - point.Z).IsZero();
	}

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
	public double GetDistanceTo(in Cartesian3dCoordinate point)
		=> (this.Point - point).CrossProduct(this.Vector).Magnitude / this.Vector.Magnitude;

	/// <summary>
	/// Gets the intersection point between this line and the provided one.
	/// </summary>
	/// <param name="line">Parametric line taht should intersect with the current one.</param>
	/// <returns>Coordinate of the intersection point between both lines.</returns>
	public Cartesian3dCoordinate GetIntersection(in ParametricLine line)
	{
		// We are calculating the R vector resulting from both line equations :
		// R = P1 + V1t1 and R = P2 + V2t2
		// P1 + V1t1 = P2 + V2t2
		// t1 = (P2 + V2t2 - P1) / V1

		// By isolating 2 coordinate, we can evaluate the following formula :
		// t1 = (X2 + A2t2 - X1) / A1 = (Y2 + B2t2 - Y1) / B1
		// t1 = (X2 + A2t2 - X1) * B1 = (Y2 + B2t2 - Y1) * A1
		// t1 = X2B1 + A2t2B1 - X1B1  = Y2A1 + B2t2A1 - Y1A1

		// Giving us the formula for t2 :
		// A2t2B1 - B2t2A1 = Y2A1 - Y1A1 - X2B1 + X1B1
		// t2(A2B1 - B2A1) = (Y2-Y1)A1 + (X1-X2)B1
		// t2 = ((Y2-Y1)A1 + (X1-X2)B1) / (A2B1 - B2A1)
		double divisor = (line.A * this.B) - (line.B * this.A);
		if (!divisor.IsZero())
		{
			double t = (((line.Y - this.Y) * this.A) + ((this.X - line.X) * this.B)) / divisor;
			var p = line.GetValueForT(t);

			// If we find a point not included in current line, both lines don't intersect.
			if (this.Contains(p))
				return p;
		}

		// If previous formula cause a division by zero, replicate the same logic on another pair of coordinate of the vector.
		divisor = (line.A * this.C) - (line.C * this.A);
		if (!divisor.IsZero())
		{
			double t = (((line.Z - this.Z) * this.A) + ((this.X - line.X) * this.C)) / divisor;
			var p = line.GetValueForT(t);

			// If we find a point not included in current line, both lines don't intersect.
			if (this.Contains(p))
				return p;
		}

		// Last possible combination
		divisor = (line.B * this.C) - (line.C * this.B);
		if (!divisor.IsZero())
		{
			double t = (((line.Z - this.Z) * this.B) + ((this.Y - line.Y) * this.C)) / divisor;
			var p = line.GetValueForT(t);

			// If we find a point not included in current line, both lines don't intersect.
			if (this.Contains(p))
				return p;
		}

		throw new GeometricException("Their is not intersection between the provided ParametricLine.");
	}

	/// <summary>
	/// Gets a ParametricLine perpendicular to the ParametricLine and passing through a provided point.
	/// </summary>
	/// <param name="point">Point through which the perpendicular line to the ParametricLine should go.</param>
	/// <returns>A new line going through the provided point and the current ParametricLine.</returns>
	public ParametricLine GetPerpendicular(in Cartesian3dCoordinate point)
	{
		// Calculate vector between Line reference point and provided point.
		Cartesian3dCoordinate v = point - this.Point;

		// By substracting the projection on the original Vector, we got the perpendicular direction.
		Cartesian3dCoordinate direction = v - v.ProjectOn(this.Vector);

		return new ParametricLine(point, direction);
	}

	/// <summary>
	/// Create a ParametricLine between 2 lines.
	/// </summary>
	/// <param name="p1">Starting point of the line.</param>
	/// <param name="p2">Ending point of the line.</param>
	/// <returns>The Parametric line linking the provided points.</returns>
	public static ParametricLine FromTwoPoints(in Cartesian3dCoordinate p1, in Cartesian3dCoordinate p2)
	{
		return new ParametricLine(p1, new Cartesian3dCoordinate(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z));
	}

	#endregion Public methods

	#region Private Methods

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private Cartesian3dCoordinate GetValueForT(double t)
		=> new(
			this.X + (this.A * t),
			this.Y + (this.B * t),
			this.Z + (this.C * t)
		);

	#endregion Private Methods
}
