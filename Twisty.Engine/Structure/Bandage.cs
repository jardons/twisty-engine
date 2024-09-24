using System.Collections.Generic;

namespace Twisty.Engine.Structure;

/// <summary>
/// Class describing all blocks bandaged as a unique movable entity.
/// </summary>
/// <param name="principal">Principal blocks used to identify the bandaged block.</param>
/// <param name="extentions">List of extensions blocks extanding the principal block.</param>
public class Bandage(Block principal, IList<Block> extentions)
{
	/// <summary>
	/// Gets the id of the principal blocks used to identify the bandaged block.
	/// </summary>
	public Block Principal { get; } = principal;

	/// <summary>
	/// Gets the list of extensions blocks ids extanding the principal block.
	/// </summary>
	public IList<Block> Extensions { get; } = extentions;
}
