using System;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Class providing extensions methods for the double manipulations.
	/// </summary>
	public static class DoubleExtensions
	{
		private const double PRECISION = 0.0000000001;

		/// <summary>
		/// Evaluate if the 2 doubles are concidered as equal in this context.
		/// </summary>
		/// <param name="d1">First double value to compare.</param>
		/// <param name="d2">Second value to compare.</param>
		/// <returns>A boolean indicating whether the 2 doubles are equals or not.</returns>
		public static bool IsEqualTo(this double d1, double d2)
		{
			return Math.Abs(d1 - d2) < PRECISION;
		}

		/// <summary>
		/// Evaluate if the this double is concidered as equal to 0.0.
		/// </summary>
		/// <param name="d1">Double value to compare.</param>
		/// <returns>A boolean indicating whether the double equals 0.0 or not.</returns>
		public static bool IsZero(this double d1)
		{
			return Math.Abs(d1) < PRECISION;
		}
	}
}