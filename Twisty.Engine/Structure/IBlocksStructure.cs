using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure;

public interface IBlocksStructure
{
	IEnumerable<Block> Blocks { get; }

	/// <summary>
	/// Gets a block using its id.
	/// </summary>
	/// <param name="blockId">Id of the block we are looking up.</param>
	/// <returns>Block for the corresponding id or null if not found.</returns>
	Block GetBlock(string blockId);

	/// <summary>
	/// Gets a block for a specified position or null if no block exist there.
	/// </summary>
	/// <param name="position">Position at which we try to find a block.</param>
	/// <returns>Block for the corresponding id or null if not found.</returns>
	Block GetBlock(Cartesian3dCoordinate position);

	/// <summary>
	/// Get a block using its original position.
	/// </summary>
	/// <param name="position">Initial position of the block relative to the core center.</param>
	/// <returns>Block for the corresponding initial position or null if not found.</returns>
	Block GetBlockForInitialPosition(Cartesian3dCoordinate position);

	/// <summary>
	/// Get the Blocks for a specific face of the twisty puzzle based on the current blocks positions.
	/// </summary>
	/// <param name="v">Orientation of the face of the puzzle.</param>
	/// <returns>The list of blocks currently visible on the requested face.</returns>
	/// <remarks>Logic is only valid for standard forms. Any shapeshifting form would need to improve this logic.</remarks>
	IEnumerable<Block> GetBlocksForFace(Cartesian3dCoordinate v);

	/// <summary>
	/// Get the Blocks for a specific face of the twisty puzzle based on the current blocks positions.
	/// </summary>
	/// <param name="faceId">Orientation of the face of the puzzle.</param>
	/// <returns>The list of blocks currently visible on the requested face.</returns>
	/// <remarks>Logic is only valid for standard forms. Any shapeshifting form would need to improve this logic.</remarks>
	IEnumerable<Block> GetBlocksForFace(string faceId);
}