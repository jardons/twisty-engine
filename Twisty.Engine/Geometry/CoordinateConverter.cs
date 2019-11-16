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
		/// Convert a Spherical Vector to his Cartesian equivalent.
		/// </summary>
		/// <param name="sc">Spherical coordinate to convert.</param>
		/// <returns>The Vector converted to a Cartesian representation.</returns>
		public static CartesianCoordinate ConvertToCartesian(SphericalVector sc)
		{
			return new CartesianCoordinate(
				Math.Sin(sc.Theta) * Math.Cos(sc.Phi),
				Math.Sin(sc.Theta) * Math.Sin(sc.Phi),
				Math.Cos(sc.Theta)
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
			if (cc.IsOnOrigin)
				return SphericalVector.Origin;

			return new SphericalVector(
				Math.Atan2(cc.Y, cc.X),
				Math.Acos(cc.Z / cc.Magnitude)
			);
		}
	}
}