using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Structure;

/// <summary>
/// Collections of bandages used for a RotationCore allowing to validate rotation operations.
/// </summary>
/// <typeparam name="T">Type of the block id.</typeparam>
public class BandagesCollection : IRotationValidator, IEnumerable<Bandage>
{
	private readonly Dictionary<string, int> m_Index;
	private readonly List<Bandage> m_Bandages;
	private readonly IBlocksStructure m_Core;

	/// <summary>
	/// Create a new BandagesCollection.
	/// </summary>
	public BandagesCollection(IBlocksStructure core)
	{
		m_Index = [];
		m_Bandages = [];
		m_Core = core;
	}

    /// <summary>
    /// Band two blocks together in the collection.
    /// </summary>
    /// <param name="principalBlockId"></param>
    /// <param name="extendedBlockId"></param>
    public void Band(string principalBlockId, string extendedBlockId)
	{
		if (m_Index.TryGetValue(principalBlockId, out int i))
		{
			m_Bandages[i].Extensions.Add(m_Core.GetBlock(extendedBlockId));
		}
		else
		{
			m_Index[principalBlockId] = m_Bandages.Count;
			m_Bandages.Add(new Bandage(m_Core.GetBlock(principalBlockId), [m_Core.GetBlock(extendedBlockId) ]));
		}
	}

    /// <summary>
    /// Band several blocks together in the collection.
    /// </summary>
    /// <param name="principalBlock"></param>
    /// <param name="extendedBlocks"></param>
    public void Band(string principalBlock, IEnumerable<string> extendedBlocks)
	{
		if (m_Index.TryGetValue(principalBlock, out int i))
		{
			foreach (var b in extendedBlocks)
				m_Bandages[i].Extensions.Add(m_Core.GetBlock(b));
		}
		else
		{
			m_Index[principalBlock] = m_Bandages.Count;
			m_Bandages.Add(new Bandage(m_Core.GetBlock(principalBlock), extendedBlocks.Select(m_Core.GetBlock).ToList()));
		}
	}

	/// <summary>
	/// Get the bandage for the specified block id.
	/// </summary>
	/// <param name="blockId"></param>
	/// <returns></returns>
	public Bandage GetBandage(string blockId)
		=> m_Index.TryGetValue(blockId, out int i)
			? m_Bandages[i]
			: null;

	#region IRotationValidator Members

	public bool CanRotateAround(RotationAxis axis, double theta, IEnumerable<string> blockIds)
	{
		var expectedList = blockIds.SelectMany(blockId =>
				 m_Index.TryGetValue(blockId, out int i)
					? m_Bandages[i].Extensions.Select(b => b.Id).Union([blockId])
					: [blockId]
			)
			.ToHashSet();

		int count = 0;
		foreach (var id in blockIds)
		{
			if (!expectedList.Contains(id)) return false;
			count++;
		}

		return count == expectedList.Count;
	}

	#endregion IRotationValidator Members

	#region IEnumerable Members

	/// <inheritdoc />
	public IEnumerator<Bandage> GetEnumerator()
		=> m_Bandages.GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator()
		=> m_Bandages.GetEnumerator();

	#endregion IEnumerable Members

}
