using System;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Converter providing conversion tools betweens Vector referentials.
	/// </summary>
	public static class CoordinateConverter
	{
		/// <summary>
		/// Convert an angle in degree to an angle in radian.
		/// </summary>
		/// <param name="degree">Angle in degree.</param>
		/// <returns>The equivalent value in radian</returns>
		public static double ConvertDegreeToRadian(int degree) => degree * (Math.PI / 180);

		/// <summary>
		/// Convert an angle in radian to an angle in degree.
		/// </summary>
		/// <param name="radian">Angle in radian.</param>
		/// <returns>The equivalent value in degree.</returns>
		public static double ConvertRadianToDegree(double radian) => radian * (180 / Math.PI);

		/// <summary>
		/// Convert a Cartesian Point to his Homogeneous equivalent.
		/// </summary>
		/// <param name="cc">Cartesian coordinate to convert.</param>
		/// <returns>The Vector converted to a Homogeneous representation.</returns>
		public static HomogeneousCoordiante ConvertToHomogeneous(CartesianCoordinate cc)
		{
			return new HomogeneousCoordiante(
				cc.X,
				cc.Y,
				cc.Z,
				1.0
			);
		}

		/// <summary>
		/// Convert a Spherical Vector to his Cartesian equivalent.
		/// </summary>
		/// <param name="sc">Spherical coordinate to convert.</param>
		/// <returns>The Vector converted to a Cartesian representation.</returns>
		public static CartesianCoordinate ConvertToCartesian(SphericalVector sc)
		{
			double sinTheta = Math.Sin(sc.Theta);

			return new CartesianCoordinate(
				sinTheta * Math.Cos(sc.Phi),
				sinTheta * Math.Sin(sc.Phi),
				Math.Cos(sc.Theta)
			);
		}

		/// <summary>
		/// Convert a Homogeneous Vector or Point to his Cartesian equivalent.
		/// </summary>
		/// <param name="hc">Homogeneous coordinate to convert.</param>
		/// <returns>The Vector converted to a Cartesian representation.</returns>
		public static CartesianCoordinate ConvertToCartesian(HomogeneousCoordiante hc)
		{
			return hc.W.IsZero()
				? new CartesianCoordinate(
					hc.X,
					hc.Y,
					hc.Z
				)
				: new CartesianCoordinate(
					hc.X / hc.W,
					hc.Y / hc.W,
					hc.Z / hc.W
				);
		}

		/// <summary>
		/// Convert a Cartesian Vector to his Spherical equivalent.
		/// </summary>
		/// <param name="cc">Cartesian coordinate to convert.</param>
		/// <returns>The Vector converted to a Spherical representation.</returns>
		public static SphericalVector ConvertToSpherical(CartesianCoordinate cc)
		{
			// Pre validate value to avoid double.NaN in calculation result.
			if (cc.IsZero)
				return SphericalVector.Origin;

			return new SphericalVector(
				Math.Atan2(cc.Y, cc.X),
				Math.Acos(cc.Z / cc.Magnitude)
			);
		}
	}
}