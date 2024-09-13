using System;
using System.Collections.Generic;

namespace Twisty.Engine.Geometry.Rotations;

/// <summary>
/// Class describing a Rotation using the rotation matrix formula.
/// </summary>
public class RotationMatrix3d
{
	/// <summary>
	/// Gets the unrotated matrix.
	/// </summary>
	public readonly static RotationMatrix3d Unrotated = new();

	/// <summary>
	/// Rotation matrix on the form of [Columns Index, Rows Index]
	/// </summary>
	private readonly double[,] m_Matrix;

	#region ctor(s)

	/// <summary>
	/// Create a unrotated rotation matrix.
	/// </summary>
	private RotationMatrix3d()
	{
		this.m_Matrix = new double[,] { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 1.0 } };
	}

	/// <summary>
	/// Create a new rotation matrix based on a rotation axis and an angle.
	/// </summary>
	/// <param name="axis">Axis around which the rotation is executed</param>
	/// <param name="angle">Angle of the clockwise rotation.</param>
	public RotationMatrix3d(in Cartesian3dCoordinate axis, double angle)
		: this(new SimpleRotation3d(axis, angle)) { }

	/// <summary>
	/// Create a new rotation matrix based on a rotation.
	/// </summary>
	/// <param name="rotation">Initial rotation applied to this matrix.</param>
	public RotationMatrix3d(in SimpleRotation3d rotation)
	{
		// Angle is reverse to provide a clockwise rotation.
		double c = Trigonometry.Cos(-rotation.Angle);
		double s = Trigonometry.Sin(-rotation.Angle);
		double t = 1.0 - c;

		// Need to use normalised vector.
		var axis = rotation.Axis.Normalize();

		// Create the matrix
		m_Matrix = new double[3, 3];

		m_Matrix[0, 0] = (c + (axis.X * axis.X * t)).AlignRatioLimits();
		m_Matrix[1, 1] = (c + (axis.Y * axis.Y * t)).AlignRatioLimits();
		m_Matrix[2, 2] = (c + (axis.Z * axis.Z * t)).AlignRatioLimits();

		double part1 = axis.X * axis.Y * t;
		double part2 = axis.Z * s;
		m_Matrix[1, 0] = (part1 + part2).AlignRatioLimits();
		m_Matrix[0, 1] = (part1 - part2).AlignRatioLimits();

		part1 = axis.X * axis.Z * t;
		part2 = axis.Y * s;
		m_Matrix[2, 0] = (part1 - part2).AlignRatioLimits();
		m_Matrix[0, 2] = (part1 + part2).AlignRatioLimits();

		part1 = axis.Y * axis.Z * t;
		part2 = axis.X * s;
		m_Matrix[2, 1] = (part1 + part2).AlignRatioLimits();
		m_Matrix[1, 2] = (part1 - part2).AlignRatioLimits();
	}

	/// <summary>
	/// Create a new rotation matrix based on a the matrix as a table format.
	/// </summary>
	/// <param name="matrix">Precalculated matrix as a table format.</param>
	private RotationMatrix3d(double[,] matrix)
	{
		if (matrix is null)
			throw new ArgumentNullException(nameof(matrix));

		if (matrix.Length != 9)
			throw new ArgumentException("Matrix size is expected to be [3, 3].");

		this.m_Matrix = matrix;
	}

	#endregion ctor(s)

	#region Public Methods

	/// <summary>
	/// Rotate the provided vector based using this rotation.
	/// </summary>
	/// <param name="cc">Cartesian COordiante of the point to rotate.</param>
	/// <returns>Rotated value.</returns>
	public Cartesian3dCoordinate Rotate(in Cartesian3dCoordinate cc)
	{
		return new Cartesian3dCoordinate(
			(cc.X * m_Matrix[0, 0]) + (cc.Y * m_Matrix[0, 1]) + (cc.Z * m_Matrix[0, 2]),
			(cc.X * m_Matrix[1, 0]) + (cc.Y * m_Matrix[1, 1]) + (cc.Z * m_Matrix[1, 2]),
			(cc.X * m_Matrix[2, 0]) + (cc.Y * m_Matrix[2, 1]) + (cc.Z * m_Matrix[2, 2]));
	}

	/// <summary>
	/// Rotate the provided vector based using this rotation.
	/// </summary>
	/// <param name="cc">Cartesian Coordinate of the point to rotate.</param>
	/// <returns>Rotated value.</returns>
	public RotationMatrix3d Rotate(in RotationMatrix3d cc)
	{
		return new RotationMatrix3d(
			new double[3, 3]
			{
				{
					(cc.m_Matrix[0, 0] * this.m_Matrix[0, 0]) + (cc.m_Matrix[0, 1] * this.m_Matrix[1, 0]) + (cc.m_Matrix[0, 2] * this.m_Matrix[2, 0]),
					(cc.m_Matrix[0, 0] * this.m_Matrix[0, 1]) + (cc.m_Matrix[0, 1] * this.m_Matrix[1, 1]) + (cc.m_Matrix[0, 2] * this.m_Matrix[2, 1]),
					(cc.m_Matrix[0, 0] * this.m_Matrix[0, 2]) + (cc.m_Matrix[0, 1] * this.m_Matrix[1, 2]) + (cc.m_Matrix[0, 2] * this.m_Matrix[2, 2])
				},
				{
					(cc.m_Matrix[1, 0] * this.m_Matrix[0, 0]) + (cc.m_Matrix[1, 1] * this.m_Matrix[1, 0]) + (cc.m_Matrix[1, 2] * this.m_Matrix[2, 0]),
					(cc.m_Matrix[1, 0] * this.m_Matrix[0, 1]) + (cc.m_Matrix[1, 1] * this.m_Matrix[1, 1]) + (cc.m_Matrix[1, 2] * this.m_Matrix[2, 1]),
					(cc.m_Matrix[1, 0] * this.m_Matrix[0, 2]) + (cc.m_Matrix[1, 1] * this.m_Matrix[1, 2]) + (cc.m_Matrix[1, 2] * this.m_Matrix[2, 2])
				},
				{
					(cc.m_Matrix[2, 0] * this.m_Matrix[0, 0]) + (cc.m_Matrix[2, 1] * this.m_Matrix[1, 0]) + (cc.m_Matrix[2, 2] * this.m_Matrix[2, 0]),
					(cc.m_Matrix[2, 0] * this.m_Matrix[0, 1]) + (cc.m_Matrix[2, 1] * this.m_Matrix[1, 1]) + (cc.m_Matrix[2, 2] * this.m_Matrix[2, 1]),
					(cc.m_Matrix[2, 0] * this.m_Matrix[0, 2]) + (cc.m_Matrix[2, 1] * this.m_Matrix[1, 2]) + (cc.m_Matrix[2, 2] * this.m_Matrix[2, 2])
				}
			}
		);
	}

	/// <summary>
	/// Gets Euler angles performing the same rotation as this matrix.
	/// Euler angles are provided in the form of up to 3 ordered rotation around the 3 principal axis.
	/// </summary>
	/// <returns>Collection of rotation in the order to execute to get the same rotation as the one represented by the current object.</returns>
	/// <remarks>
	/// Multiple euler angles can be returned for a same rotation matrix, only one solution will be generated here.
	/// </remarks>
	public IReadOnlyList<SimpleRotation3d> GetEulerAngles()
	{
		List<SimpleRotation3d> result = [];

		// Generate rotation aroung main angles.
		// Psi(ψ) : rotation around X axis.
		// Thetha(θ) : rotation aroung Y axis.
		// Phi(φ) : rotation around Z axis.

		// Giving R = Rz(φ)Ry(θ)Rx(ψ)
		//     [ cosθ cosφ     sinψ sinθ cosφ − cosψ sinφ     cosψ sinθ cosφ + sinψ sinφ ]
		// R = [ cosθ sinφ     sinψ sinθ sinφ + cosψ cosφ     cosψ sinθ sinφ − sinψ cosφ ]
		//     [   -sinθ               sinψ cosθ                        cosψ cosθ        ]

		// As    R31 = -sinθ,
		// Then  θ = -asin(R31)
		var theta = -Trigonometry.Asin(this.m_Matrix[0, 2]);

		var cosTheta = Trigonometry.Cos(theta);
		if (!cosTheta.IsZero())
		{
			// Following method is not valid if cosθ is equal to zero as it would result in a divide per 0.

			// When theta is positive :
			// As    R32/R33 = tanψ,
			// Then  ψ = atan2(R32,R33)
			// We can generalize to negative theta with :
			// ψ = atan2(R32 / cosθ,R33 / cosθ)
			var psi = Math.Atan2(this.m_Matrix[1, 2] / cosTheta, this.m_Matrix[2, 2] / cosTheta);

			// When theta is positive :
			// As    R21/R11 = tanφ,
			// Then  φ = atan2(R21,R11)
			// We can generalize to negative theta with :
			// φ = atan2(R21 / cosθ,R11 / cosθ)
			var phi = Math.Atan2(this.m_Matrix[0, 1] / cosTheta, this.m_Matrix[0, 0] / cosTheta);

			// Add operation to result
			if (!phi.IsZero())
				result.Add(new SimpleRotation3d(Cartesian3dCoordinate.ZAxis, phi));
			if (!theta.IsZero())
				result.Add(new SimpleRotation3d(Cartesian3dCoordinate.YAxis, theta));
			if (!psi.IsZero())
				result.Add(new SimpleRotation3d(Cartesian3dCoordinate.XAxis, psi));
		}
		else
		{
			// Using this solution when cosθ is equal to zero would lead to Gimbal lock, causing the matrix to look like :
			//     [    0.0      sinψ sinθ cosφ − cosψ sinφ     cosψ sinθ cosφ + sinψ sinφ ]
			// R = [    0.0      sinψ sinθ sinφ + cosψ cosφ     cosψ sinθ sinφ − sinψ cosφ ]
			//     [   -sinθ                0.0                              0.0           ]

			// By combination of R12 and R13 equations, we got :
			// When R31 = 1 : ψ = −φ + atan2(−R12,−R13)
			// When R31 = -1 : ψ = φ + atan2(R12,R13)
			//
			// As this situation lead to an infinite of ψ and φ combination, we will concentrate on the one with φ set to 0.0.
			var psi = this.m_Matrix[0, 2] > 0.0
				? Math.Atan2(-this.m_Matrix[1, 0], -this.m_Matrix[2, 0])
				: Math.Atan2(this.m_Matrix[1, 0], this.m_Matrix[2, 0]);

			// Add operation to result
			if (!theta.IsZero())
				result.Add(new SimpleRotation3d(Cartesian3dCoordinate.YAxis, theta));
			if (!psi.IsZero())
				result.Add(new SimpleRotation3d(Cartesian3dCoordinate.XAxis, psi));
		}

		return result.AsReadOnly();
	}

	#endregion Public Methods
}