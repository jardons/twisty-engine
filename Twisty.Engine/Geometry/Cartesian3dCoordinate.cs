using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Class providing cartesian coordinates representation of a vector in 3 dimensions.
	/// Vector coordinates are representated by the combination of coordinates following the 3 perpendicular axis X, Y and Z.
	/// </summary>
	/// <example>
	/// Diagram :
	/// 
	///            +z   -x
	///             |  /
	///             | /
	///             |/
	///   -y -------O------- +y
	///            /|
	///           / |
	///          /  |
	///        +x   -z
	///          
	/// </example>
	[DebuggerDisplay("({X}, {Y}, {Z})")]
	public struct Cartesian3dCoordinate
	{
		/// <summary>
		/// Gets the Zero point coordinates.
		/// </summary>
		public static readonly Cartesian3dCoordinate Zero = new Cartesian3dCoordinate(0.0, 0.0, 0.0);

		/// <summary>
		/// Gets the Positive X Axis.
		/// </summary>
		public static readonly Cartesian3dCoordinate XAxis = new Cartesian3dCoordinate(1.0, 0.0, 0.0);

		/// <summary>
		/// Gets the Positive Y Axis.
		/// </summary>
		public static readonly Cartesian3dCoordinate YAxis = new Cartesian3dCoordinate(0.0, 1.0, 0.0);

		/// <summary>
		/// Gets the Positive Z Axis.
		/// </summary>
		public static readonly Cartesian3dCoordinate ZAxis = new Cartesian3dCoordinate(0.0, 0.0, 1.0);

		#region ctor(s)

		/// <summary>
		/// Create a new Cartesian3dCoordinate from a coordinates string on the format "(X Y Z)".
		/// </summary>
		/// <param name="coordinates">Coordinates in the format "(X Y Z)".</param>
		public Cartesian3dCoordinate(string coordinates)
		{
			try
			{
				double[] parsed = CoordinateConverter.ParseCoordinates(coordinates);
				if (parsed.Length != 3)
					throw new ArgumentException("The provided coordinates are not in the expected format '(X Y Z)' and does not contains 3 values.", nameof(coordinates));

				this.X = parsed[0];
				this.Y = parsed[1];
				this.Z = parsed[2];
			}
			catch (FormatException e)
			{
				throw new ArgumentException("The provided coordinates are not in the expected format '(X Y Z)' and cannot be parsed.", nameof(coordinates), e);
			}
		}

		/// <summary>
		/// Create a new Cartesian3dCoordinate object.
		/// </summary>
		/// <param name="x">Coordinates on the X axis.</param>
		/// <param name="y">Coordinates on the Y axis.</param>
		/// <param name="z">Coordinates on the Z axis.</param>
		public Cartesian3dCoordinate(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
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
		/// Gets the cartesian coordinate on the Z axis.
		/// </summary>
		public double Z { get; }

		/// <summary>
		/// Gets the magnitude of this vector, also noted as ||V||.
		/// </summary>
		/// <remarks>
		/// Formula :
		///          _________________
		/// ||V|| = V Xv² * Yv² * Zv² '
		/// </remarks>
		public double Magnitude => Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z));

		/// <summary>
		/// Gets the angle in radians between the vector and the X axis.
		/// </summary>
		public double ThetaToX => Trigonometry.Acos(this.X / this.Magnitude);

		/// <summary>
		/// Gets the angle in radians between the vector and the Y axis.
		/// </summary>
		public double ThetaToY => Trigonometry.Acos(this.Y / this.Magnitude);

		/// <summary>
		/// Gets the angle in radians between the vector and the Z axis.
		/// </summary>
		public double ThetaToZ => Trigonometry.Acos(this.Z / this.Magnitude);

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is on the X axis or not.
		/// </summary>
		public bool IsOnX => Y.IsZero() && Z.IsZero();

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is on the Y axis or not.
		/// </summary>
		public bool IsOnY => X.IsZero() && Z.IsZero();

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is on the Z axis or not.
		/// </summary>
		public bool IsOnZ => X.IsZero() && Y.IsZero();

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is the origin point crossing all axes or not.
		/// </summary>
		public bool IsZero => X.IsZero() && Y.IsZero() && Z.IsZero();

		/// <summary>
		/// Gets the Reverse vector of this one.
		/// </summary>
		public Cartesian3dCoordinate Reverse => new Cartesian3dCoordinate(-this.X, -this.Y, -this.Z);

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Create a new vector resulting from a rotation around the X axis.
		/// </summary>
		/// <param name="theta">Angle fo rotation to execute in radians.</param>
		/// <returns>The coordinates of the rotated vector.</returns>
		/// <remarks>
		/// Rotation formula :
		/// 
		///              (  1      0           0      )   (  x  )
		///  Rx(theta) = (  0  cos theta  -sin theta  ) X (  y  )
		///              (  0  sin theta   cos theta  )   (  z  )
		/// </remarks>
		public Cartesian3dCoordinate RotateAroundX(double theta)
		{
			// Precalculate intermediates values used more than once.
			double cosTheta = Trigonometry.Cos(theta);
			double sinTheta = Trigonometry.Sin(theta);

			return new Cartesian3dCoordinate(
				this.X,
				(this.Y * cosTheta) - (this.Z * sinTheta),
				(this.Y * sinTheta) + (this.Z * cosTheta)
			);
		}

		/// <summary>
		/// Create a new vector resulting from a rotation around the Y axis.
		/// </summary>
		/// <param name="theta">Angle fo rotation to execute in radians.</param>
		/// <returns>The coordinates of the rotated vector.</returns>
		/// <remarks>
		/// Rotation formula :
		/// 
		///              (   cos theta  0  sin theta  )   (  x  )
		///  Ry(theta) = (       0      1      0      ) X (  y  )
		///              (  -sin theta  0  cos theta  )   (  z  )
		/// </remarks>
		public Cartesian3dCoordinate RotateAroundY(double theta)
		{
			// Precalculate intermediates values used more than once.
			double cosTheta = Trigonometry.Cos(theta);
			double sinTheta = Trigonometry.Sin(theta);

			return new Cartesian3dCoordinate(
				(this.X * cosTheta) + (this.Z * sinTheta),
				this.Y,
				(this.Z * cosTheta) - (this.X * sinTheta)
			);
		}

		/// <summary>
		/// Create a new vector resulting from a rotation around the Z axis.
		/// </summary>
		/// <param name="theta">Angle fo rotation to execute in radians.</param>
		/// <returns>The coordinates of the rotated vector.</returns>
		/// <remarks>
		/// Rotation formula :
		/// 
		///              (  cos theta  -sin theta  0  )   (  x  )
		///  Rz(theta) = (  sin theta   cos theta  0  ) X (  y  )
		///              (      0           0      1  )   (  z  )
		/// </remarks>
		public Cartesian3dCoordinate RotateAroundZ(double theta)
		{
			// Precalculate intermediates values used more than once.
			double cosTheta = Trigonometry.Cos(theta);
			double sinTheta = Trigonometry.Sin(theta);

			return new Cartesian3dCoordinate(
				(this.X * cosTheta) - (this.Y * sinTheta),
				(this.X * sinTheta) + (this.Y * cosTheta),
				this.Z
			);
		}

		/// <summary>
		/// Create a new vector resulting from a rotation around a specific axis using the rodrigues' formula.
		/// </summary>
		/// <param name="k">Vector used as the rotation axis.</param>
		/// <param name="theta">Angle fo rotation to execute in radians.</param>
		/// <returns>The coordinates of the rotated vector.</returns>
		/// <remarks>
		/// Rotation formula :
		/// 
		///	 V : This vector.
		///	 K : Vector used a rotation axis.
		/// 
		///  R = V cos theta + (K × V) sin theta + K (K . V) (1 − cos theta)
		/// </remarks>
		public Cartesian3dCoordinate RotateAroundVector(Cartesian3dCoordinate k, double theta)
		{
			// Precalculate intermediates values used more than once.
			double cosTheta = Trigonometry.Cos(theta);
			double sinTheta = Trigonometry.Sin(theta);

			k = k.Normalize();

			return (this * cosTheta) + ((-sinTheta) * CrossProduct(k)) + ((1.0 - cosTheta) * this.DotProduct(k) * k);
		}

		/// <summary>
		/// Get the vector corresponding to this vector in the current referential as if this vector was related to another referential vector.
		/// </summary>
		/// <param name="referential">Vector correspoding to the X axis vector in the referential coordinates.</param>
		/// <returns>A new coordinates sets in the global coordinates system corresponding to the translated coordinates.</returns>
		/// <remarks>
		/// Assuming a transposition of A on B.
		/// 
		/// Let V = a X b
		/// Let s = ||V|| (sin of angle)
		/// Let c = A . B (cos of angle)
		/// 
		/// Rotation matrix can be calculated by:
		/// 
		/// R = I + [V]x + [V]×² (1 − c) / s²
		/// 
		/// As a reminder:
		/// 
		///        (  0  -Vz  Vy )         ( 1 0 0 )               (  1  -Vz  Vy )
		/// [V]x = (  Vz  0  -Vx )     I = ( 0 1 0 )    [V]x + I = (  Vz  1  -Vx )
		///        ( -Vy  Vx  0  )         ( 0 0 1 )               ( -Vy  Vx  1  )
		/// </remarks>
		public Cartesian3dCoordinate TransposeFromReferential(Cartesian3dCoordinate referential)
		{
			if (referential.IsOnX)
			{
				if (referential.X > 0.0)
					// No need to change referential when we are on the correct one.
					return this;
				else
					// Formula is not working for reverse referential vector, we just reverse the current coordinates.
					return this.Reverse;
			}

			Cartesian3dCoordinate origin = XAxis;

			// Calculate formula variables.
			Cartesian3dCoordinate v = origin.CrossProduct(referential);
			double c = origin.DotProduct(referential);

			// Calculate last part.
			double cPart = (1.0 - c) / ((v.X * v.X) + (v.Y * v.Y) + (v.Z * v.Z));

			// Calculate poer values.
			double x2 = v.X * v.X;
			double y2 = v.Y * v.Y;
			double z2 = v.Z * v.Z;

			// Calculate the rotation matrix
			double[,] matrix = new double[3, 3]
			{
				{
					1.0 - ((z2 + y2) * cPart),
					(v.Y * -v.X * cPart) - v.Z,
					(v.Z * v.X * cPart) + v.Y,
				},
				{
					(v.X * v.Y * cPart) + v.Z,
					1.0 - ((z2 + x2) * cPart),
					(v.Z * v.Y * cPart) - v.X,
				},
				{
					(v.X * v.Z * cPart) - v.Y,
					(v.Y * v.Z * cPart) + v.X,
					1.0 - ((y2 + x2) * cPart),
				},
			};

			// Apply rotation matrix to vector.
			return new Cartesian3dCoordinate(
				(this.X * matrix[0, 0]) + (this.Y * matrix[0, 1]) + (this.Z * matrix[0, 2]),
				(this.X * matrix[1, 0]) + (this.Y * matrix[1, 1]) + (this.Z * matrix[1, 2]),
				(this.X * matrix[2, 0]) + (this.Y * matrix[2, 1]) + (this.Z * matrix[2, 2]));
		}

		/// <summary>
		/// Gets the projection of the current verctor on another provided vector.
		/// </summary>
		/// <param name="vector">Vector providing the direction on which the original vector will be projected.</param>
		/// <returnsThe point coordinate resulting of the projection of this vector on the provided vector.</returns>
		public Cartesian3dCoordinate ProjectOn(Cartesian3dCoordinate vector)
			=> this.DotProduct(vector) / vector.DotProduct(vector) * vector;

		/// <summary>
		/// Gets the normalized coordinate of this vector.
		/// </summary>
		/// <returns>A new Cartesian3dCoordinate containing the normalized coordinate of this vector.</returns>
		public Cartesian3dCoordinate Normalize()
		{
			double m = this.Magnitude;
			if (m > 0.0)
				return new Cartesian3dCoordinate(this.X / m, this.Y / m, this.Z / m);

			return this;
		}

		/// <summary>
		/// Calculate the theta angle relative to a provided vector.
		/// </summary>
		/// <param name="x">Vector that will be used to calculate the theta</param>
		/// <returns>Calculated theta value between the two vectors.</returns>
		/// <remarks>
		/// Formula :
		/// 
		///  V : This vector.
		///  X : Vector to which the angle theta is calculated.
		/// 
		///                  V . X
		///  cos theta = -------------
		///               ||X|| ||Y||
		/// </remarks>
		public double GetThetaTo(Cartesian3dCoordinate x)
		{
			return Trigonometry.Acos(this.DotProduct(x) / (this.Magnitude * x.Magnitude));
		}

		/// <summary>
		/// Gets the dot product between this vector in a 1 X 3 format and a second one in a 3 X 1 format.
		/// </summary>
		/// <param name="c">Vector that will be used as a 3 X 1 matrix.</param>
		/// <returns>Calculated vector from the dot product of the 2 vectors.</returns>
		public double DotProduct(Cartesian3dCoordinate c)
		{
			return (this.X * c.X) + (this.Y * c.Y) + (this.Z * c.Z);
		}

		/// <summary>
		/// Gets the Cross Product between this vector and another vector.
		/// </summary>
		/// <param name="v">Vector with which we will calculate the cross product.</param>
		/// <returns>New coordinates containing the cross product from the 2 vectors.</returns>
		public Cartesian3dCoordinate CrossProduct(Cartesian3dCoordinate v)
		{
			return new Cartesian3dCoordinate(
				(this.Y * v.Z) - (this.Z * v.Y),
				(this.Z * v.X) - (this.X * v.Z),
				(this.X * v.Y) - (this.Y * v.X)
			);
		}

		/// <summary>
		/// Evaluate if the 2 Cartesian3dCoordinate are concidered as equal points in this context.
		/// </summary>
		/// <param name="cc">Coordinates to compare to the current object.</param>
		/// <returns>A boolean indicating whether the 2 coordinates are equals or not.</returns>
		public bool IsSamePoint(Cartesian3dCoordinate cc) => this.X.IsEqualTo(cc.X) && this.Y.IsEqualTo(cc.Y) && this.Z.IsEqualTo(cc.Z);

		/// <summary>
		/// Evaluate if the 2 Cartesian3dCoordinate are concidered as equal vector in this context.
		/// </summary>
		/// <param name="cc">Coordinates to compare to the current object.</param>
		/// <returns>A boolean indicating whether the 2 coordinates are equals or not.</returns>
		public bool IsSameVector(Cartesian3dCoordinate cc) => this.Normalize().IsSamePoint(cc.Normalize());

		#endregion Public Methods

		#region Operators

		/// <summary>
		/// Gets the result of the substraction of 2 vectors.
		/// </summary>
		/// <param name="v1">First vector from which the second one will be substracted.</param>
		/// <param name="v2">Substracted Vector.</param>
		/// <returns>Result of the subbstraction of the 2 vectors.</returns>
		public static Cartesian3dCoordinate operator -(Cartesian3dCoordinate v1, Cartesian3dCoordinate v2)
		{
			return new Cartesian3dCoordinate(
					v1.X - v2.X,
					v1.Y - v2.Y,
					v1.Z - v2.Z
				);
		}

		/// <summary>
		/// Gets the sum of 2 vectors.
		/// </summary>
		/// <param name="v1">First vector to add.</param>
		/// <param name="v2">Second vector to add.</param>
		/// <returns>Result of the sum of the 2 vectors.</returns>
		public static Cartesian3dCoordinate operator +(Cartesian3dCoordinate v1, Cartesian3dCoordinate v2)
		{
			return new Cartesian3dCoordinate(
					v1.X + v2.X,
					v1.Y + v2.Y,
					v1.Z + v2.Z
				);
		}

		/// <summary>
		/// Gets the product of a vector with a double value.
		/// </summary>
		/// <param name="v1">First vector to multiply.</param>
		/// <param name="v">Value to multiply with all value fo the vector.</param>
		/// <returns>Result of the product of the vector with the value.</returns>
		public static Cartesian3dCoordinate operator *(double v, Cartesian3dCoordinate v1)
		{
			return new Cartesian3dCoordinate(
					v1.X * v,
					v1.Y * v,
					v1.Z * v
				);
		}

		/// <summary>
		/// Gets the product of a vector with a double value.
		/// </summary>
		/// <param name="v1">First vector to multiply.</param>
		/// <param name="v2">Value to multiply with all value fo the vector.</param>
		/// <returns>Result of the product of the vector with the value.</returns>
		public static Cartesian3dCoordinate operator *(Cartesian3dCoordinate v1, double v)
		{
			return new Cartesian3dCoordinate(
					v1.X * v,
					v1.Y * v,
					v1.Z * v
				);
		}

		#endregion Operators
	}
}