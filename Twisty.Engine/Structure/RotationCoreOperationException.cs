using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Structure
{
	public class RotationCoreOperationException : Exception
	{
		public RotationCoreOperationException(string message)
			: base(message) { }
	}
}
