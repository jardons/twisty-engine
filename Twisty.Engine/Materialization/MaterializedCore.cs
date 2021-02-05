using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Materialization
{
	/// <summary>
	/// Model object representing the coordinates allowing to represent a RotationCore in 3d.
	/// </summary>
	public class MaterializedCore
	{
		/// <summary>
		/// Create a new MaterializedCore from the list of his internal objects.
		/// </summary>
		/// <param name="objects">Collection of object forming the Core.</param>
		internal MaterializedCore(IEnumerable<MaterializedObject> objects)
			=> this.Objects = objects;

		/// <summary>
		/// Gets the collection of objects present in the MaterializedCore.
		/// </summary>
		public IEnumerable<MaterializedObject> Objects { get; }
	}
}
