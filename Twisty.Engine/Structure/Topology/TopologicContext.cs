using System.Collections.ObjectModel;

namespace Twisty.Engine.Structure.Topology;

/// <summary>
/// Class describing the creation context information for a TopologyMap.
/// </summary>
public class TopologicContext
{
	private readonly Dictionary<string, int> m_Index;

	/// <summary>
	/// Create a new TopologicContext.
	/// </summary>
	/// <param name="topologicIds">Collectin of available TopologicId avialable in the context.</param>
	public TopologicContext(IEnumerable<string> topologicIds)
	{
		TopologicIds = topologicIds.OrderBy(id => id).ToList().AsReadOnly();
		m_Index = [];
		for (int i = 0; i < TopologicIds.Count; ++i)
		{
			m_Index[TopologicIds[i]] = i;
		}
	}

	/// <summary>
	/// Gets the integer index for the provided TopologicId.
	/// </summary>
	/// <param name="topologicId">TopologicId search in this Context.</param>
	/// <returns>The index of the reuqested TopologicId in the TopologicIds table of this Context.</returns>
	/// <exception cref="KeyNotFoundException">Exception is throwned when the requested TopologicId don't exist in this Context.</exception>
	public int GetTopologicIdIndex(string topologicId)
		=> m_Index.TryGetValue(topologicId, out var id)
			? id
			: throw new KeyNotFoundException("TopologicId doesn't exist in this context.");

	/// <summary>
	/// Gets the collection of possible TopologicIds available in this Context.
	/// </summary>
	public ReadOnlyCollection<string> TopologicIds { get; }
}
