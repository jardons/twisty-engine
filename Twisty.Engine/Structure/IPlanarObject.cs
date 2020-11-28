using System;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Common interface to objects for which coordinates are defined by a Plane.
	/// </summary>
	public interface IPlanarObject
	{
		/// <summary>
		/// Gets the Coordinates of the Plane representing the object.
		/// </summary>
		public Plane Plane { get; }
	}
}
