using System;
using System.Diagnostics;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Geometry
{
	/// <summary>
	/// Common interface to objects for which coordinates are defined by a Plane.
	/// </summary>
	public interface IPlanar
	{
		/// <summary>
		/// Gets the Coordinates of the Plane representing the object.
		/// </summary>
		public Plane Plane { get; }
	}
}
