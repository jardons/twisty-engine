using Moq;
using System;
using System.Collections.Generic;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Xunit;

namespace Twisty.Engine.Tests.Structure;

[Trait("Category", "Structure")]
public class BandagesCollectionTest
{
	#region Test Methods

	[Fact]
	public void CanRotateAroundWithNoBlock_True()
	{
		// 1. Prepare
		var axis = new RotationAxis("test", Cartesian3dCoordinate.XAxis);
		Block[] blocks = [];
		var bandages = new BandagesCollection(GetBlocksStructure(blocks).Object);

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, blocks.Select(b => b.Id));

		// 3. Verify
		Assert.True(b);
	}

	[Fact]
	public void CanRotateAroundWithOneBlock_True()
	{
		// 1. Prepare
		var axis = new RotationAxis("test", Cartesian3dCoordinate.XAxis);
		Block[] blocks = [GetBlock("b0", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))];
		var bandages = new BandagesCollection(GetBlocksStructure(blocks).Object);

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, blocks.Select(b => b.Id));

		// 3. Verify
		Assert.True(b);
	}

	[Fact]
	public void CanRotateAroundWithAllBandagedBlockMoving_True()
	{
		// 1. Prepare
		var axis = new RotationAxis("test", Cartesian3dCoordinate.XAxis);
		Block[] blocks = [
			GetBlock("b0", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis)),
			GetBlock("b1", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))
		];

		var bandages = new BandagesCollection(GetBlocksStructure(blocks).Object);
		bandages.Band(blocks[0].Id, blocks[1].Id);

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, blocks.Select(b => b.Id));

		// 3. Verify
		Assert.True(b);
	}

	[Fact]
	public void CanRotateAroundWithAllBandagedBlockNotMoving_True()
	{
		// 1. Prepare
		var axis = new RotationAxis("test", Cartesian3dCoordinate.XAxis);
		Block[] rotatedBlocks = [
			GetBlock("b2", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))
		];
		Block[] bandagedBlocks = [
			GetBlock("b0", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis)),
			GetBlock("b1", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))
		];

		var bandages = new BandagesCollection(GetBlocksStructure(rotatedBlocks.Union(bandagedBlocks).ToArray()).Object);
		bandages.Band(bandagedBlocks[0].Id, bandagedBlocks[1].Id);

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, rotatedBlocks.Select(b => b.Id));

		// 3. Verify
		Assert.True(b);
	}

	[Fact]
	public void CanRotateAroundWithMissingBandagedBlock_False()
	{
		// 1. Prepare
		var axis = new RotationAxis("test", Cartesian3dCoordinate.XAxis);
		Block[] rotatedBlocks = [
			GetBlock("b2", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))
		];
		Block[] bandagedBlocks = [
			GetBlock("b0", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))
		];

		var bandages = new BandagesCollection(GetBlocksStructure(rotatedBlocks.Union(bandagedBlocks).ToArray()).Object);
		bandages.Band(rotatedBlocks[0].Id, bandagedBlocks[0].Id);

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, rotatedBlocks.Select(b => b.Id));

		// 3. Verify
		Assert.False(b);
	}

	#endregion Test Methods

	private static Block GetBlock(string id, Cartesian3dCoordinate coordinates, params BlockFace[] blockFaces)
		=> new(new BlockDefinition(id, coordinates, blockFaces));

	private Mock<IBlocksStructure> GetBlocksStructure(Block[] blocks)
	{
		var m = new Mock<IBlocksStructure>();

		foreach (var b in blocks)
		{
			m.Setup(o => o.GetBlock(b.Id)).Returns(b);
			m.Setup(o => o.GetBlock(b.Definition.InitialPosition)).Returns(b);
		}

		m.SetupGet(o => o.Blocks).Returns(blocks);

		return m;
	}
}