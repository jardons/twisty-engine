using System.Collections.Generic;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Operations.Rubiks
{
	/// <summary>
	/// Interface providing cube command parsing operations.
	/// </summary>
	/// <typeparam name="T">Tye of ROtationCOre for which this parser is implemented.</typeparam>
	public interface IOperationsParser<T>
		where T : RotationCore
	{
		/// <summary>
		/// Parse a command in a list of operations
		/// </summary>
		/// <param name="command">String that will be parsed as a list of operations.</param>
		/// <returns>List of operations to execute on the ROtationCOre to performe the provided command.</returns>
		IEnumerable<IOperation<T>> Parse(string command);
	}
}