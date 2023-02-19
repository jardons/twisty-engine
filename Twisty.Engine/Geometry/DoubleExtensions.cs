using System;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Class providing extensions methods for the double manipulations.
	/// </summary>
	public static class DoubleExtensions
	{
		#region Const Members

		private const double PRECISION = 0.0000000001;

		#endregion Const Members

		/// <summary>
		/// Evaluate if the 2 doubles are concidered as equal in this context.
		/// </summary>
		/// <param name="d1">First double value to compare.</param>
		/// <param name="d2">Second value to compare.</param>
		/// <returns>A boolean indicating whether the 2 doubles are equals or not.</returns>
		/// <remarks>
		/// Note that this function is designed to compare small double.
		/// It's usage is fair enough for the current project but can not be seen as a correct generic solution.
		/// </remarks>
		public static bool IsEqualTo(this double d1, double d2) => Math.Abs(d1 - d2) < PRECISION;

		/// <summary>
		/// Evaluate if the this double is concidered as equal to 0.0.
		/// </summary>
		/// <param name="d1">Double value to compare.</param>
		/// <returns>A boolean indicating whether the double equals 0.0 or not.</returns>
		/// <remarks>
		/// Note that this function is designed to compare small double.
		/// It's usage is fair enough for the current project but can not be seen as a correct generic solution.
		/// </remarks>
		public static bool IsZero(this double d1) => Math.Abs(d1) < PRECISION;

		/// <summary>
		/// Align the current double on the ratio limits to avoid precisions issues.
		/// </summary>
		/// <param name="d">Double value to align on the ratio limits</param>
		/// <returns>
		/// Aligned boolean aligned between -1 and 1.
		/// Zero values will also be rounded to 0.0 to clean loss of precision.
		/// </returns>
		public static double AlignRatioLimits(this double d)
		{
			if (d.IsZero())
				return 0.0;

			if (d > 1.0)
				return 1.0;

			if (d < -1.0)
				return -1.0;

			return d;
		}
	}
}