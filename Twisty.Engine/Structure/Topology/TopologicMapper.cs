using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure.Topology;

public class TopologicMapper(IBlocksStructure Structure, ITopologyBuilder Builder)
{
	private TopologicContext m_Context;

	/// <summary>
	/// Gets the current TopologicMap of the Structure.
	/// </summary>
	/// <returns></returns>
	public TopologicMap GetTopologicMap()
	{
		var blocks = Structure.Blocks.Where(b => b.Bandage is null || ReferenceEquals(b, b.Bandage.Principal)).ToList();

		blocks.Sort(new PositionPointComparer());
		var topologicIds = blocks.Select(Builder.GetTopologicId).ToArray();

		m_Context ??= new TopologicContext(topologicIds.Distinct());

		return new TopologicMap(m_Context, topologicIds);
	}
}