using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Materialization
{
	/// <summary>
	/// Model object representing the coordinates allowing to represent a RotationCore object in 3d.
	/// </summary>
	public class MaterializedObject
	{
		/// <summary>
		/// Create a new MaterializedObject from the list of his internal parts.
		/// </summary>
		/// <param name="id">ID of the object.</param>
		/// <param name="parts">Collection of parts forming the object.</param>
		internal MaterializedObject(string id, IEnumerable<MaterializedObjectPart> parts)
		{
			this.Id = id;
			this.Parts = parts;
		}

		/// <summary>
		/// Gets the collection of Parts forming the object.
		/// </summary>
		public IEnumerable<MaterializedObjectPart> Parts { get; }

		/// <summary>
		/// Gets the Id of the object.
		/// </summary>
		public string Id { get; }
	}
}
