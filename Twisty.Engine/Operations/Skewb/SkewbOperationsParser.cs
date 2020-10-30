using System;
using System.Collections.Generic;
using Twisty.Engine.Structure.Skewb;

namespace Twisty.Engine.Operations.Skewb
{
	/// <summary>
	/// Class designed to parse a list of operations to perform on a skewb cube.
	/// </summary>
	public class RubikOperationsParser : IOperationsParser<SkewbCube>
	{
		/// <summary>
		/// Parse command and generate an Operation list based on its content.
		/// </summary>
		/// <param name="command">Command string containing a list of operations to execute on the cube.</param>
		/// <returns>The collection containing the operations to execute based on the command content.</returns>
		/// <exception cref="Twisty.Engine.Operations.OperationParsingException">The provided command contains invalid characters that cannot be parsed.</exception>
		public IEnumerable<IOperation<SkewbCube>> Parse(string command)
		{
			if (string.IsNullOrEmpty(command))
				return Array.Empty<IOperation<SkewbCube>>();

			List<IOperation<SkewbCube>> operations = new List<IOperation<SkewbCube>>();
			for (int i = 0; i < command.Length; ++i)
			{
				var c = command[i];
				if (c == ' ' || c == '\t' || c == '\n' || c == '(' || c == ')')
					continue;

				string axis = GetAxisId(command[i]);
				if (string.IsNullOrEmpty(axis))
					throw new OperationParsingException(command, i);
				
				// Counter clockwise operation are followed by a "'" char.
				bool isClockWise = true;
				if (i + 1 < command.Length && command[i + 1] == '\'')
				{
					++i;
					isClockWise = false;
				}

				operations.Add(new SkewbOperation(axis, isClockWise));
			}

			return operations;
		}

		/// <summary>
		/// Gets the axis id used for the specific action.
		/// </summary>
		/// <param name="action">Action used for which the axis is needed.</param>
		/// <returns>Id of the axis to use for the action.</returns>
		private string GetAxisId(char action)
		{
			switch (action)
			{
				case 'L':
					return "DFL";
				case 'R':
					return "DBR";
				/*case 'F':
					return RubikCube.FACE_ID_FRONT;*/
				case 'B':
					return "DBL";
				case 'U':
					return "UBL";
				/*case 'D':
					return RubikCube.FACE_ID_DOWN;*/
			}

			return null;
		}
	}
}
