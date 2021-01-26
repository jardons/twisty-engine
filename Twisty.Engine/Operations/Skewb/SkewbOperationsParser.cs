using System;
using System.Collections.Generic;
using Twisty.Engine.Structure.Skewb;

namespace Twisty.Engine.Operations.Skewb
{
	/// <summary>
	/// Class designed to parse a list of operations to perform on a skewb cube.
	/// </summary>
	public class SkewbOperationsParser : IOperationsParser
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

				string axis = GetAxisId(command[i]);
				if (string.IsNullOrEmpty(axis))
					throw new OperationParsingException(command, i);
				
				// Counter clockwise operation are followed by a "'" char.
				bool isClockwise = true;
				if (i + 1 < command.Length && command[i + 1] == '\'')
				{
					++i;
					isClockwise = false;
				}

				operations.Add(new AxisOperation(axis, isClockwise ? Math.PI * (2.0 / 3.0) : -Math.PI * (2.0 / 3.0)));
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
			// Lower case chars are not used in skewb logic, so we can switch all chars to upper cases.
			cleanedCommand = command.ToUpper();

			try
			{
				this.Parse(cleanedCommand);
				return true;
			}
			catch
			{
				cleanedCommand = command;
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
				'L' => "DFL",
				'R' => "DBR",
				'B' => "DBL",
				'U' => "UBL",
				_ => null,
			};
		}
	}
}
