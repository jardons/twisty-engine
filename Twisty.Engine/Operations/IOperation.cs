using Twisty.Engine.Structure;

namespace Twisty.Engine.Operations;

/// <summary>
/// Interface used to provide basic operations on the twist core engine.
/// </summary>
public interface IOperation
{
	/// <summary>
	/// Gets the operation unique Id.
	/// </summary>
	string Id { get; }

	/// <summary>
	/// Execute the current operation on the provided IRotatable core.
	/// </summary>
	/// <param name="core">IRotatable core on which the operation is executed.</param>
	/// <returns>A boolean indicating if whether the operation was succesful or not.</returns>
	bool ExecuteOn(IRotatable core);
}
