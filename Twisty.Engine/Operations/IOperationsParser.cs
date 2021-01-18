using System.Collections.Generic;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Operations
{
	/// <summary>
	/// Interface providing twisty puzzles command parsing generic operations.
	/// </summary>
	public interface IOperationsParser
	{
		/// <summary>
		/// Try to clean a command to a generic format.
		/// </summary>
		/// <param name="command">String that will be parsed as a list of operations.</param>
		/// <param name="cleanedCommand">String that will be contained a cleaned list of operations.</param>
		/// <returns>A boolean indicating if whether the provided string is a valid command or not.</returns>
		bool TryClean(string command, out string cleanedCommand);
	}

	/// <summary>
	/// Interface providing cube command parsing operations.
	/// </summary>
	/// <typeparam name="T">Type of RotationCore for which this parser is implemented.</typeparam>
	public interface IOperationsParser<T> : IOperationsParser
		where T : RotationCore
	{
		/// <summary>
		/// Parse a command in a list of operations.
		/// </summary>
		/// <param name="command">String that will be parsed as a list of operations.</param>
		/// <returns>List of operations to execute on the ROtationCOre to performe the provided command.</returns>
		IEnumerable<IOperation<T>> Parse(string command);
	}
}