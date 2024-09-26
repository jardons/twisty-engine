using System.Collections.ObjectModel;

namespace Twisty.Engine.Structure.Topology;

/// <summary>
/// Class providing the map of the Topology for a specific Blocks structure.
/// </summary>
public class TopologicMap
{
	/// <summary>
	/// Create a enw TopologicMap.
	/// </summary>
	/// <param name="context">Context of creation for the current TopologicMap.</param>
	/// <param name="topologicIds">Oredered list of TopologicId in the TopologicContext.</param>
	public TopologicMap(TopologicContext context, IEnumerable<string> topologicIds)
	{
		Context = context;
		TopologicIds = topologicIds
			.Select(context.GetTopologicIdIndex)
			.ToList()
			.AsReadOnly();
	}

	/// <summary>
	/// Gets the context of creation for the current TopologicMap.
	/// </summary>
	public TopologicContext Context { get; }

	/// <summary>
	/// Gets the Oredered list of TopologicId index in the TopologicContext.
	/// </summary>
	public ReadOnlyCollection<int> TopologicIds { get; }

	/// <summary>
	/// Gets the TopologicFormat string.
	/// </summary>
	/// <returns>Return the TopologicFormat string.</returns>
	public string GetTopologicMapFormat()
		=> string.Join(',', TopologicIds);
}
