using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Engine.Operations
{
	public interface IOperationRunner
	{
		/// <summary>
		/// Execute a set of operation on the current rotatable core.
		/// </summary>
		/// <param name="operations">Ordered collection of operations to run on the core object.</param>
		void Execute(IEnumerable<IOperation> operations);

		/// <summary>
		/// Execute a set of operation on the current rotatable core.
		/// </summary>
		/// <param name="operation">Operation to run on the core object.</param>
		void Execute(IOperation operation);
	}
}
