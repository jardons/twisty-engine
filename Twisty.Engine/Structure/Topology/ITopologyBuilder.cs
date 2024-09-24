namespace Twisty.Engine.Structure.Topology;

/// <summary>
/// Interface proposing methods allowing to generate topologic information.
/// </summary>
public interface ITopologyBuilder
{
	/// <summary>
	/// Generate the TopologicId for the provided Block.
	/// </summary>
	/// <param name="block">Block for which the TopologicId is generated.</param>
	/// <param name="structure">Structure containing the blocks and its potential extensions.</param>
	/// <returns>A TopologicId uniquely describing a block structure.</returns>
	string GetTopologicId(Block block);
}