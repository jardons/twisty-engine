using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Analysis;

namespace Twisty.Engine.Tests.Structure.Analysis;

public class DifferenceRatioAnalyzerTest
{
	[Theory]
	[InlineData("(1 0 0)")]
	[InlineData("(1 1 1)")]
	public void ResolutionAnalyzer_GetAlterationForSameInstance_GetUnchanged(string facePosition)
	{
		// 1. Prepare
		Cartesian3dCoordinate cc = new(facePosition);
		Block b = new(new BlockDefinition("lonely", Cartesian3dCoordinate.XAxis, new BlockFace("face", cc)));
		var s = CreateStructure(b);
		DifferenceRatioAnalyzer a = new(s);

		// 2. Execute
		var r = a.GetDifferenceRatios(s);

		// 3. Verify
		Assert.Equal(1, r.UnchangedCount);
		Assert.Equal(0, r.RotatedCount);
		Assert.Equal(0, r.ReplacedCount);
		Assert.Equal(0, r.MovedCount);
	}

	#region Private Members

	private static IBlocksStructure CreateStructure(Block b, Block originalBlock = null)
	{
		Mock<IBlocksStructure> mock = new();

		List<Block> blocks = new() { b };
		if (originalBlock is not null)
		{
			blocks.Add(originalBlock);
			mock.Setup(o => o.GetBlock(It.IsAny<Cartesian3dCoordinate>())).Returns(originalBlock);
		}
		else
			mock.Setup(o => o.GetBlock(It.IsAny<Cartesian3dCoordinate>())).Returns(b);

		mock.Setup(o => o.GetBlock(It.IsAny<string>())).Returns(b);
		mock.SetupGet(o => o.Blocks).Returns(blocks);

		return mock.Object;
	}

	#endregion Private Members
}
