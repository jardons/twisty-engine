using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Materialization;

namespace Twisty.Runner.Models.Model3d
{
	/// <summary>
	/// 3d object to diplay in a WPF control.
	/// </summary>
	public class Core3dObject
	{
		/// <summary>
		/// Create a new Core3dObject from a MaterializedObject.
		/// </summary>
		/// <param name="o">MaterializedObject used to create the Core3dObject.</param>
		public Core3dObject(MaterializedObject o)
		{
			this.Id = o.Id;
			var parts = new List<Core3dSurface>();
			this.Parts = parts;
			foreach (var p in o.Parts)
				parts.Add(new Core3dSurface(p));
		}

		/// <summary>
		/// Gets the Id of this object.
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// Gets the colleciton of parts composing this object.
		/// </summary>
		public IEnumerable<Core3dSurface> Parts { get; }
	}
}
