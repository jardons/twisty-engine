using System;
using System.Collections.Generic;
using Twisty.Engine.Structure.Rubiks;

namespace Twisty.Engine.Operations.Rubiks
{
	/// <summary>
	/// Class designed to parse a list of operations to perform on a rubiks cube.
	/// </summary>
	public class RubikOperationsParser : IOperationsParser<RubikCube>
	{
		/// <summary>
		/// Parse command and generate an Operation list based on its content.
		/// </summary>
		/// <param name="command">Command string containing a list of operations to execute on the cube.</param>
		/// <returns>The collection containing the operations to execute based on the command content.</returns>
		/// <exception cref="Twisty.Engine.Operations.OperationParsingException">The provided command contains invalid characters that cannot be parsed.</exception>
		public IEnumerable<IOperation<RubikCube>> Parse(string command)
		{
			if (string.IsNullOrEmpty(command))
				return Array.Empty<IOperation<RubikCube>>();

			List<IOperation<RubikCube>> operations = new List<IOperation<RubikCube>>();
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

				operations.Add(new RubikOperation(axis, isClockWise));
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
					return RubikCube.ID_FACE_LEFT;
				case 'R':
					return RubikCube.ID_FACE_RIGHT;
				case 'F':
					return RubikCube.ID_FACE_FRONT;
				case 'B':
					return RubikCube.ID_FACE_BACK;
				case 'U':
					return RubikCube.ID_FACE_UP;
				case 'D':
					return RubikCube.ID_FACE_DOWN;
			}

			return null;
		}
	}
}
