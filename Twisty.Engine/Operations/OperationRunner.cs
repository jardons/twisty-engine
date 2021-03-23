using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Operations
{
	/// <summary>
	/// Create a OperationRunner allowing to execute operation on a rotatable core object.
	/// </summary>
	public class OperationRunner : IOperationRunner
	{
		private readonly IRotatable m_Core;

		public OperationRunner(IRotatable core)
		{
			m_Core = core;
		}

		/// <summary>
		/// Execute a set of operation on a provided rotatable core.
		/// </summary>
		/// <param name="operations">Ordered collection of operations to run on the core object.</param>
		public void Execute(IEnumerable<IOperation> operations)
		{
			foreach (IOperation operation in operations)
				operation.ExecuteOn(m_Core);
		}

		/// <summary>
		/// Execute a set of operation on a provided rotatable core.
		/// </summary>
		/// <param name="operation">Operation to run on the core object.</param>
		public void Execute(IOperation operation)
			 => operation.ExecuteOn(m_Core);
	}
}
