using System;
using System.Globalization;

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
		/// Parse a string of coordinates to get
		/// </summary>
		/// <param name="coordinates">String containing the coordinates to parse.</param>
		/// <returns>A table of double containing the coordinate in the given order.</returns>
		public static double[] ParseCoordinates(string coordinates)
		{
			if (coordinates == null)
				throw new ArgumentNullException(nameof(coordinates));

			if (!coordinates.StartsWith("(") || !coordinates.EndsWith(")") || coordinates.Length < 3)
				throw new FormatException("Coordiantes doesn't follow the expected format '(DDD.D, DDD.D, DDD.D)' where 'DDD.D' must be a valid Double.");

			string[] values = coordinates.Substring(1, coordinates.Length - 2).Split(' ', StringSplitOptions.RemoveEmptyEntries);
			double[] result = new double[values.Length];

			for (int i = 0; i < values.Length; ++i)
				if (double.TryParse(values[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
					result[i] = d;
				else
					throw new FormatException("Coordiantes doesn't follow the expected format '(DDD.D, DDD.D, DDD.D)' where 'DDD.D' must be a valid Double.");

			return result;
		}

		/// <summary>
		/// Convert a Cartesian Point to his Homogeneous equivalent.
		/// </summary>
		/// <param name="cc">Cartesian coordinate to convert.</param>
		/// <returns>The Vector converted to a Homogeneous representation.</returns>
		public static HomogeneousCoordinate ConvertToHomogeneous(Cartesian3dCoordinate cc)
		{
			return new HomogeneousCoordinate(
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
		public static Cartesian3dCoordinate ConvertToCartesian(SphericalVector sc)
		{
			double sinTheta = Math.Sin(sc.Theta);

			return new Cartesian3dCoordinate(
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
		public static Cartesian3dCoordinate ConvertToCartesian(HomogeneousCoordinate hc)
		{
			return hc.W.IsZero()
				? new Cartesian3dCoordinate(
					hc.X,
					hc.Y,
					hc.Z
				)
				: new Cartesian3dCoordinate(
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
		public static SphericalVector ConvertToSpherical(Cartesian3dCoordinate cc)
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