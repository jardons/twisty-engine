using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Operations
{
	/// <summary>
	/// Interface used to provide basic operations on the twist core engine.
	/// </summary>
	public interface IOperation<T>
		where T : RotationCore
	{
		/// <summary>
		/// Execute the current operation on the provided Core.
		/// </summary>
		/// <param name="core">Rotation core on which the operation is executed.</param>
		void ExecuteOn(T core);
	}
}
