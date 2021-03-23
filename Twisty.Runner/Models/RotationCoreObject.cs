using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Operations;
using Twisty.Engine.Structure;

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
		/// <param name="core">RotationCore represented in this object.</param>
		/// <param name="runner">Operation Runner used to perform rotations on the Rotation Core.</param>
		public RotationCoreObject(RotationCore core, IOperationRunner runner)
		{
			this.Core = core;
			this.Runner = runner;
		}

		#endregion ctor(s)

		#region Public Properties

		/// <summary>
		/// Gets the Operation Runner used to perform rotations on the Rotation Core.
		/// </summary>
		public IOperationRunner Runner { get; }

		/// <summary>
		/// Gets the RotationCore represented in this object.
		/// </summary>
		public RotationCore Core { get; }

		#endregion Public Properties
	}
}
