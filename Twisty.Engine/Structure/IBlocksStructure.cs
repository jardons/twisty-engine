using System.Collections.Generic;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	public interface IBlocksStructure
	{
		IEnumerable<Block> Blocks { get; }

		Block GetBlock(string blockId);
		Block GetBlockForInitialPosition(Cartesian3dCoordinate position);
		IEnumerable<Block> GetBlocksForFace(Cartesian3dCoordinate v);
		IEnumerable<Block> GetBlocksForFace(string faceId);
	}
}