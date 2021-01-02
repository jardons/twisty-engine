using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Static class providing trigonometry operations with error margin handling.
	/// </summary>
	public static class Trigonometry
	{
		/// <summary>
		/// Calculate the arc cosinus for a value.
		/// </summary>
		/// <param name="d">Cosinus value used to calculate angle.</param>
		/// <returns>Calculated Arc cosinus value.</returns>
		public static double Acos(double d)
		{
			// Avoid NaN coming from double precissions issues. (1.0000000000001)
			if (d.IsEqualTo(1.0))
				return 0.0;

			return Math.Acos(d);
		}

		/// <summary>
		/// Calculate the arc sinus for a value.
		/// </summary>
		/// <param name="d">Sinus value used to calculate angle.</param>
		/// <returns>Calculated Arc sinus value.</returns>
		public static double Asin(double d)
		{
			// Avoid NaN coming from double precissions issues. (1.0000000000001)
			if (d.IsEqualTo(1.0))
				return Math.PI / 2.0;

			return Math.Asin(d);
		}

		/// <summary>
		/// Calculate the Cosinus for an angle in radian by avoiding precision lost for some case causing important rounding issues with our matrix.
		/// </summary>
		/// <param name="rad">Angle to evaluate in radians.</param>
		/// <returns>Result value of the Cos operation.</returns>
		public static double Cos(double rad)
		{
			// Math.Cos calculates wrong results for values larger than 1e6
			rad = NormarizeAngle(rad);

			// Return perfect 0.0 when possible to limit double precisions issues.
			double piResult = rad / Math.PI;
			if (piResult.IsEqualTo(0.5) || piResult.IsEqualTo(-0.5) || piResult.IsEqualTo(1.5) || piResult.IsEqualTo(-1.5))
				return 0.0;

			return Math.Cos(rad);
		}

		/// <summary>
		/// Calculate the Sinus for an angle in radian by avoiding precision lost for some case causing important rounding issues with our matrix.
		/// </summary>
		/// <param name="rad">Angle to evaluate in radians.</param>
		/// <returns>Result value of the Sin operation.</returns>
		public static double Sin(double rad)
		{
			// Math.Sin calculates wrong results for values larger than 1e6
			rad = NormarizeAngle(rad);

			// Return perfect 0.0 when possible to limit double precisions issues.
			if (rad.IsZero() || rad.IsEqualTo(Math.PI) || rad.IsEqualTo(-Math.PI))
				return 0.0;

			return Math.Sin(rad);
		}

		/// <summary>
		/// Normalize angle to a value within a single circle rotation.
		/// </summary>
		/// <param name="rad">Angle to evaluate in radians.</param>
		/// <returns>Normalized angle value with a value lower than 2*Pi.</returns>
		private static double NormarizeAngle(double rad)
		{
			return rad % (2.0 * Math.PI);
		}
	}
}
