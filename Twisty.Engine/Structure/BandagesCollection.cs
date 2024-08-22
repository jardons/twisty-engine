using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Structure;

public class BandagesCollection : IRotationValidator
{
	private readonly Dictionary<string, int> m_Index;
	private readonly List<Bandage> m_Bandages;

	public BandagesCollection()
	{
		m_Index = [];
		m_Bandages = [];
	}

	public void Band(Block principalBlock, Block extendedBlock)
	{
		if (m_Index.TryGetValue(principalBlock.Id, out int i))
		{
			m_Bandages[i].Extensions.Add(extendedBlock);
		}
		else
		{
			m_Index[principalBlock.Id] = m_Bandages.Count;
			m_Bandages.Add(new Bandage(principalBlock, [extendedBlock]));
		}
	}

	public Bandage GetBandage(string blockId)
		=> m_Index.TryGetValue(blockId, out int i)
			? m_Bandages[i]
			: null;

	#region IRotationValidator Members

	public bool CanRotateAround(RotationAxis axis, double theta, IEnumerable<Block> blocks)
	{
		var expectedList = blocks.SelectMany(b =>
				 m_Index.TryGetValue(b.Id, out int i)
					? m_Bandages[i].Extensions.Union([ b ])
					: [ b ]
			)
			.Select(b => b.Id)
			.ToHashSet();

		int count = 0;
		foreach (var b in blocks)
		{
			if (!expectedList.Contains(b.Id)) return false;
			count++;
		}

		return count == expectedList.Count;
	}

	#endregion IRotationValidator Members

}
