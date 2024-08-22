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
		var bandages = new BandagesCollection();
		Block[] blocks = [];

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, blocks);

		// 3. Verify
		Assert.True(b);
	}

	[Fact]
	public void CanRotateAroundWithOneBlock_True()
	{
		// 1. Prepare
		var axis = new RotationAxis("test", Cartesian3dCoordinate.XAxis);
		var bandages = new BandagesCollection();
		Block[] blocks = [new Block("b0", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))];

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, blocks);

		// 3. Verify
		Assert.True(b);
	}

	[Fact]
	public void CanRotateAroundWithAllBandagedBlock_True()
	{
		// 1. Prepare
		var axis = new RotationAxis("test", Cartesian3dCoordinate.XAxis);
		var bandages = new BandagesCollection();

		Block[] blocks = [
			new Block("b0", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis)),
			new Block("b1", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))
		];

		bandages.Band(blocks[0], blocks[1]);

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, blocks);

		// 3. Verify
		Assert.True(b);
	}

	[Fact]
	public void CanRotateAroundWithMissingBandagedBlock_False()
	{
		// 1. Prepare
		var axis = new RotationAxis("test", Cartesian3dCoordinate.XAxis);
		var bandages = new BandagesCollection();
		Block[] blocks = [new Block("b0", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis))];

		bandages.Band(blocks[0], new Block("b1", Cartesian3dCoordinate.XAxis, new BlockFace("face", Cartesian3dCoordinate.XAxis)));

		// 2. Execute
		var b = bandages.CanRotateAround(axis, Math.PI, blocks);

		// 3. Verify
		Assert.False(b);
	}

	#endregion Test Methods
}