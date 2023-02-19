using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twisty.Engine.Structure.Analysis
{
	public struct DifferenceRatios
	{
		public UInt16 UnchangedCount;
		public UInt16 RotatedCount;
		public UInt16 ReplacedCount;
		public UInt16 MovedCount;
	}

	public class DifferenceRatioAnalyzer
	{
		private readonly IBlocksStructure m_OriginalBlocks;

		public DifferenceRatioAnalyzer(IBlocksStructure originalStructure)
		{
			m_OriginalBlocks = originalStructure;
		}

		public DifferenceRatios GetDifferenceRatios(IBlocksStructure targetStructure)
		{
			DifferenceRatios ratios = new DifferenceRatios();

			foreach (var block in m_OriginalBlocks.Blocks)
			{
				var targetBlock = targetStructure.GetBlock(block.Position);
				if (targetBlock is null)
				{
					// Block has been moved away from vurrent position without any replacement.
					ratios.MovedCount++;
					continue;
				}

				switch (block.GetAlteration(targetBlock))
				{
					case AlterationType.Orientation:
						ratios.RotatedCount++;
						continue;
					case AlterationType.Position:
						ratios.ReplacedCount++;
						continue;
				}

				// Reaching the end means no alteration was detected.
				ratios.UnchangedCount++;
			}

			return ratios;
		}
	}
}
