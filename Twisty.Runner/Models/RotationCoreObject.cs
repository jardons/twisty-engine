using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Operations;
using Twisty.Engine.Structure;
using Twisty.Runner.Models.Model3d;

namespace Twisty.Runner.Models
{
	/// <summary>
	/// Generic object containg Rotation Core information.
	/// </summary>
	public class RotationCoreObject
	{
		#region ctor(s)

		/// <summary>
		/// Create a new RotationCoreObject object.
		/// </summary>
		/// <param name="id">Id of the rotation core.</param>
		/// <param name="core">RotationCore represented in this object.</param>
		/// <param name="runner">Operation Runner used to perform rotations on the Rotation Core.</param>
		/// <param name="core3D">3D description of the core object.</param>
		public RotationCoreObject(string id, RotationCore core, IOperationRunner runner, Core3d core3D)
		{
			this.Id = id;
			this.Core = core;
			this.Runner = runner;
			this.VisualObject = core3D;
		}

		#endregion ctor(s)

		#region Public Properties

		/// <summary>
		/// Gets the id of the Rotation Core.
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// Gets the Operation Runner used to perform rotations on the Rotation Core.
		/// </summary>
		public IOperationRunner Runner { get; }

		/// <summary>
		/// Gets the RotationCore represented in this object.
		/// </summary>
		public RotationCore Core { get; }

		/// <summary>
		/// Gets the 2d description of the Core object.
		/// </summary>
		public Core3d VisualObject { get; set; } 

		#endregion Public Properties
	}
}
