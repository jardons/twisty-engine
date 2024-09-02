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
public class BandagesCollection<T> : IRotationValidator<T>, IEnumerable<Bandage<T>>
{
	private readonly Dictionary<T, int> m_Index;
	private readonly List<Bandage<T>> m_Bandages;

	/// <summary>
	/// Create a new BandagesCollection.
	/// </summary>
	public BandagesCollection()
	{
		m_Index = [];
		m_Bandages = [];
	}

    /// <summary>
    /// Band two blocks together in the collection.
    /// </summary>
    /// <param name="principalBlockId"></param>
    /// <param name="extendedBlock"></param>
    public void Band(T principalBlockId, T extendedBlockId)
	{
		if (m_Index.TryGetValue(principalBlockId, out int i))
		{
			m_Bandages[i].Extensions.Add(extendedBlockId);
		}
		else
		{
			m_Index[principalBlockId] = m_Bandages.Count;
			m_Bandages.Add(new Bandage<T>(principalBlockId, [ extendedBlockId ]));
		}
	}

    /// <summary>
    /// Band several blocks together in the collection.
    /// </summary>
    /// <param name="principalBlockId"></param>
    /// <param name="extendedBlocksIds"></param>
    public void Band(T principalBlockId, IEnumerable<T> extendedBlocksIds)
	{
		if (m_Index.TryGetValue(principalBlockId, out int i))
		{
			foreach (var b in extendedBlocksIds)
				m_Bandages[i].Extensions.Add(b);
		}
		else
		{
			m_Index[principalBlockId] = m_Bandages.Count;
			m_Bandages.Add(new Bandage<T>(principalBlockId, extendedBlocksIds.ToList()));
		}
	}

	/// <summary>
	/// Get the bandage for the specified block id.
	/// </summary>
	/// <param name="blockId"></param>
	/// <returns></returns>
	public Bandage<T> GetBandage(T blockId)
		=> m_Index.TryGetValue(blockId, out int i)
			? m_Bandages[i]
			: null;

	#region IRotationValidator Members

	public bool CanRotateAround(RotationAxis axis, double theta, IEnumerable<T> blockIds)
	{
		var expectedList = blockIds.SelectMany(id =>
				 m_Index.TryGetValue(id, out int i)
					? m_Bandages[i].Extensions.Union([ id ])
					: [ id ]
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
	public IEnumerator<Bandage<T>> GetEnumerator()
		=> m_Bandages.GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator()
		=> m_Bandages.GetEnumerator();

	#endregion IEnumerable Members

}
