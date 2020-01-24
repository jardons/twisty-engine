using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class defining a face of the puzzle.
	/// When a twisty puzle is in its solved state, all block face should be oriented on a CoreFace.
	/// </summary>
	public class CoreFace
	{
		/// <summary>
		/// Create a new CoreFace for a specific Plane.
		/// </summary>
		/// <param name="id">Id of the face.</param>
		/// <param name="p">PLane representing the face of the Core.</param>
		public CoreFace(string id, Plane p)
		{
			this.Coordinates = p;
			this.Id = id;
		}

		/// <summary>
		/// Gets the Coordinates of the Plane representing the face.
		/// </summary>
		public Plane Coordinates { get; }

		/// <summary>
		/// Gets the Id of the CoreFace.
		/// </summary>
		public string Id { get; }
	}
}