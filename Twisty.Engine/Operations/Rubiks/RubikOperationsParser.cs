using System;
using System.Collections.Generic;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;

namespace Twisty.Engine.Operations.Rubiks
{
	/// <summary>
	/// Class designed to parse a list of operations to perform on a rubiks cube.
	/// </summary>
	public class RubikOperationsParser : IOperationsParser
	{
		/// <summary>
		/// Parse command and generate an Operation list based on its content.
		/// </summary>
		/// <param name="command">Command string containing a list of operations to execute on the cube.</param>
		/// <returns>The collection containing the operations to execute based on the command content.</returns>
		/// <exception cref="Twisty.Engine.Operations.OperationParsingException">The provided command contains invalid characters that cannot be parsed.</exception>
		public IEnumerable<IOperation> Parse(string command)
		{
			if (string.IsNullOrEmpty(command))
				return Array.Empty<IOperation>();

			List<IOperation> operations = new List<IOperation>();
			for (int i = 0; i < command.Length; ++i)
			{
				var c = command[i];
				if (c == ' ' || c == '\t' || c == '\n' || c == '(' || c == ')')
					continue;

				string axis = GetAxisId(c);
				if (string.IsNullOrEmpty(axis))
					throw new OperationParsingException(command, i);
				
				double rotation = Math.PI / 2.0;
				if (i + 1 < command.Length && command[i + 1] == '\'')
				{
					// Counter clockwise operation are followed by a "'" char.
					++i;
					rotation = -rotation;
				}
				else if (i + 1 < command.Length && command[i + 1] == '2')
				{
					// Half turn operation are followeb by a "2" char.
					++i;
					rotation = Math.PI;
				}

				operations.Add(new LayerOperation(axis, rotation, new LayerInterval(0)));
			}

			return operations;
		}

		/// <summary>
		/// Try to clean a command to a generic format.
		/// </summary>
		/// <param name="command">String that will be parsed as a list of operations.</param>
		/// <param name="cleanedCommand">String that will be contained a cleaned list of operations.</param>
		/// <returns>A boolean indicating if whether the provided string is a valid command or not.</returns>
		public bool TryClean(string command, out string cleanedCommand)
		{
			cleanedCommand = command;

			try
			{
				this.Parse(cleanedCommand);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the axis id used for the specific action.
		/// </summary>
		/// <param name="action">Action used for which the axis is needed.</param>
		/// <returns>Id of the axis to use for the action.</returns>
		private string GetAxisId(char action)
		{
			return action switch
			{
				'L' => RubikCube.ID_FACE_LEFT,
				'R' => RubikCube.ID_FACE_RIGHT,
				'F' => RubikCube.ID_FACE_FRONT,
				'B' => RubikCube.ID_FACE_BACK,
				'U' => RubikCube.ID_FACE_UP,
				'D' => RubikCube.ID_FACE_DOWN,
				_ => null,
			};
		}
	}
}
