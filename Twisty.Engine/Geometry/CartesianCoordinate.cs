using System;
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
	///         /   |
	///        +x   -z
	///          
	/// </example>
	[DebuggerDisplay("({X}, {Y}, {Z})")]
	public struct CartesianCoordinate
	{
		/// <summary>
		/// Gets the Zero point coordinates.
		/// </summary>
		public static readonly CartesianCoordinate Zero = new CartesianCoordinate(0.0, 0.0, 0.0);

		/// <summary>
		/// Create a new CartesianCoordinate object.
		/// </summary>
		/// <param name="x">Coordinates on the X axis.</param>
		/// <param name="y">Coordinates on the Y axis.</param>
		/// <param name="z">Coordinates on the Z axis.</param>
		public CartesianCoordinate(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
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
		/// Gets the cartesian coordinate on the Z axis.
		/// </summary>
		public double Z { get; }

		/// <summary>
		/// Gets the magnitude of this vector.
		/// </summary>
		public double Magnitude => Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);

		/// <summary>
		/// Gets the angle in radians between the vector and the X axis.
		/// </summary>
		public double ThetaToX => Math.Acos(this.X / this.Magnitude);

		/// <summary>
		/// Gets the angle in radians between the vector and the Y axis.
		/// </summary>
		public double ThetaToY => Math.Acos(this.Y / this.Magnitude);
		
		/// <summary>
		/// Gets the angle in radians between the vector and the Z axis.
		/// </summary>
		public double ThetaToZ => Math.Acos(this.Z / this.Magnitude);

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is on the X axis or not.
		/// </summary>
		public bool IsOnX => AreEqual(Y, 0.0) && AreEqual(Z, 0.0);

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is on the Y axis or not.
		/// </summary>
		public bool IsOnY => AreEqual(X, 0.0) && AreEqual(Z, 0.0);

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is on the Z axis or not.
		/// </summary>
		public bool IsOnZ => AreEqual(X, 0.0) && AreEqual(Y, 0.0);

		/// <summary>
		/// Gets a boolean indicating if whether the current coordonate is the origin point crossing all axes or not.
		/// </summary>
		public bool IsOnOrigin => AreEqual(X, 0.0) && AreEqual(Y, 0.0) && AreEqual(Z, 0.0);

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
		public CartesianCoordinate RotateAroundX(double theta)
		{
			// Precalculate intermediates values used more than once.
			double cosTheta = Cos(theta);
			double sinTheta = Sin(theta);

			// Create matrix.
			double[,] matrix = new double[3, 3]
			{
				{ 1.0, 0.0, 0.0 },
				{ 0.0, cosTheta, -sinTheta },
				{ 0.0, sinTheta, cosTheta },
			};

			return MultiplicateByMatrix(matrix);
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
		public CartesianCoordinate RotateAroundY(double theta)
		{
			// Precalculate intermediates values used more than once.
			double cosTheta = Cos(theta);
			double sinTheta = Sin(theta);

			// Create matrix.
			double[,] matrix = new double[3, 3]
			{
				{ cosTheta, 0.0, sinTheta },
				{ 0.0, 1.0, 0.0 },
				{ -sinTheta, 0.0, cosTheta },
			};

			return MultiplicateByMatrix(matrix);
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
		public CartesianCoordinate RotateAroundZ(double theta)
		{
			// Precalculate intermediates values used more than once.
			double cosTheta = Cos(theta);
			double sinTheta = Sin(theta);

			// Create matrix.
			double[,] matrix = new double[3, 3]
			{
				{ cosTheta, -sinTheta, 0.0 },
				{ sinTheta, cosTheta, 0.0 },
				{ 0.0, 0.0, 1.0 },
			};

			return MultiplicateByMatrix(matrix);
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
		///  R = V cos theta + (K × V) sin theta + K (K . V) (1 − cos theta)
		/// </remarks>
		public CartesianCoordinate RotateAroundVector(CartesianCoordinate k, double theta)
		{
			// Precalculate intermediates values used more than once.
			double cosTheta = Cos(theta);
			double sinTheta = Sin(theta);

			k = k.Normalize();

			return this * cosTheta + (-sinTheta) * CrossProduct(k) + (1.0 - cosTheta) * this.DotProduct(k) * k;
		}

		/// <summary>
		/// Gets the normalized coordinate of this vector.
		/// </summary>
		/// <returns>A new CartesianCoordinate containing the normalized coordinate of this vector.</returns>
		public CartesianCoordinate Normalize()
		{
			double m = this.Magnitude;
			if (m > 0.0)
				return new CartesianCoordinate(this.X / m, this.Y / m, this.Z / m);

			return this;
		}

		#endregion Public Methods

		#region Operators

		/// <summary>
		/// Gets the sum of 2 vectors.
		/// </summary>
		/// <param name="v1">First vector to add.</param>
		/// <param name="v2">Secnod vector to add.</param>
		/// <returns>Result of the sum of the 2 vectors.</returns>
		public static CartesianCoordinate operator +(CartesianCoordinate v1, CartesianCoordinate v2)
		{
			return new CartesianCoordinate(
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
		public static CartesianCoordinate operator *(double v, CartesianCoordinate v1)
		{
			return new CartesianCoordinate(
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
		public static CartesianCoordinate operator *(CartesianCoordinate v1, double v)
		{
			return new CartesianCoordinate(
					v1.X * v,
					v1.Y * v,
					v1.Z * v
				);
		}

		#endregion Operators

		#region Private Methods

		/// <summary>
		/// Evaluate if the 2 doubles are concidered as equal in this context.
		/// </summary>
		/// <param name="d1">First double value to compare.</param>
		/// <param name="d2">Second value to compare.</param>
		/// <returns>A boolean indicating whether the 2 doubles are equals or not.</returns>
		private bool AreEqual(double d1, double d2)
		{
			return Math.Abs(d1 - d2) < 0.0000000001;
		}

		/// <summary>
		/// Normalize angle to a value within a single circle rotation.
		/// </summary>
		/// <param name="rad">Angle to evaluate in radians.</param>
		/// <returns>Normalized angle value with a value lower than 2*Pi.</returns>
		private double NormarizeAngle(double rad)
		{
			return rad % (2 * Math.PI);
		}

		/// <summary>
		/// Calculate the Cosinus for an angle in radian by avoiding precision lost for some case causing important rounding issues with our matrix.
		/// </summary>
		/// <param name="rad">Angle to evaluate in radians.</param>
		/// <returns>Result value of the Cos operation.</returns>
		private double Cos(double rad)
		{
			// Math.Cos calculates wrong results for values larger than 1e6
			rad = NormarizeAngle(rad);

			// Return perfect 0.0 when possible to limit double precisions issues.
			double piResult = rad / Math.PI;
			if (AreEqual(piResult, 0.5) || AreEqual(piResult, -0.5) || AreEqual(piResult, 1.5) || AreEqual(piResult, -1.5))
				return 0.0;

			return Math.Cos(rad);
		}

		/// <summary>
		/// Calculate the Sinus for an angle in radian by avoiding precision lost for some case causing important rounding issues with our matrix.
		/// </summary>
		/// <param name="rad">Angle to evaluate in radians.</param>
		/// <returns>Result value of the Sin operation.</returns>
		private double Sin(double rad)
		{
			// Math.Sin calculates wrong results for values larger than 1e6
			rad = NormarizeAngle(rad);

			// Return perfect 0.0 when possible to limit double precisions issues.
			if (AreEqual(rad, 0) || AreEqual(rad, Math.PI) || AreEqual(rad, -Math.PI))
				return 0.0;

			return Math.Sin(rad);
		}

		/// <summary>
		/// Gets the dot product between this vector in a 1 X 3 format and a second one in a 3 X 1 format.
		/// </summary>
		/// <param name="c">Vector that will be used as a 3 X 1 matrix.</param>
		/// <returns>Calculated vector from the dot product of the 2 vectors.</returns>
		private double DotProduct(CartesianCoordinate c)
		{
			return this.X * c.X + this.Y * c.Y + this.Z * c.Z;
		}

		/// <summary>
		/// Multiplicate a matrix to the current vector.
		/// </summary>
		/// <param name="matrix"></param>
		/// <returns></returns>
		private CartesianCoordinate MultiplicateByMatrix(double[,] matrix)
		{
			return new CartesianCoordinate(
				this.X * matrix[0, 0] + this.Y * matrix[0, 1] + this.Z * matrix[0, 2],
				this.X * matrix[1, 0] + this.Y * matrix[1, 1] + this.Z * matrix[1, 2],
				this.X * matrix[2, 0] + this.Y * matrix[2, 1] + this.Z * matrix[2, 2]
			);
		}

		/// <summary>
		/// Gets the Cross Product between 2 vectors.
		/// </summary>
		/// <param name="matrix"></param>
		/// <returns></returns>
		private CartesianCoordinate CrossProduct(CartesianCoordinate v)
		{
			return new CartesianCoordinate(
				this.Y * v.Z - this.Z * v.Y,
				this.Z * v.X - this.X * v.Z,
				this.X * v.Y - this.Y * v.X
			);
		}

		#endregion Private Methods
	}
}