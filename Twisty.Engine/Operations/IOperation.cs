using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Operations
{
	/// <summary>
	/// Interface used to provide basic operations on the twist core engine.
	/// </summary>
	public interface IOperation
	{
		/// <summary>
		/// Execute the current operation on the provided IRotable core.
		/// </summary>
		/// <param name="core">IRotable core on which the operation is executed.</param>
		void ExecuteOn(IRotable core);
	}
}
