using System.Collections.Generic;

namespace Twisty.Engine.Structure;

/// <summary>
/// Class describing all blocks bandaged as a unique movable entity.
/// </summary>
/// <param name="principal">Principal blocks used to identify the bandaged block.</param>
/// <param name="extentions">List of extensions blocks extanding the principal block.</param>
/// <typeparam name="T">Type of the block id.</typeparam>
public class Bandage<T>(T principal, IList<T> extentions)
{
	/// <summary>
	/// Gets the id of the principal blocks used to identify the bandaged block.
	/// </summary>
	public T Principal { get; } = principal;

	/// <summary>
	/// Gets the list of extensions blocks ids extanding the principal block.
	/// </summary>
	public IList<T> Extensions { get; } = extentions;
}
