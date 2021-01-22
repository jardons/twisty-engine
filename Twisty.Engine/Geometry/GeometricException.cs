using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Exception raised when a Geometric operation is impossible.
	/// </summary>
	public class GeometricException : Exception
	{
		/// <summary>
		/// Create a new GeometricException.
		/// </summary>
		/// <param name="message">Message of the exception.</param>
		public GeometricException(string message)
			: base(message)
		{
		}
	}
}
