using System;
using System.Linq;
using System.Collections.Generic;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Tests.DataFactories;

namespace Twisty.Engine.Tests.Structure;

[Trait("Category", "Structure")]
public class BlockTest
{
	#region Test Methods

	[Fact]
	public void CreateWithNullDefinition_ThrowArgumentNullException()
	{
		// 1. Prepare
		Block b;

		// 2. Execute
		void a() => b = new(null);

		// 3. Verify
		Assert.Throws<ArgumentNullException>(a);
	}

	[Fact]
	public void CreateAndGetFace_ShouldFindFace()
	{
		// 1. Prepare
		SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);

		// 2. Execute
		Block b = new(new BlockDefinition("id", Cartesian3dCoordinate.XAxis, new BlockFace("test", faceOrientation)));
		BlockFace f = b.GetBlockFace(faceOrientation);

		// 3. Verify
		Assert.NotNull(f);
	}

	[Theory]
	// Base Axis Rotations
	[InlineData("(0 0 1)", Math.PI / 2.0, "(1 0 0)", "(0 -1 0)")]
	[InlineData("(0 0 1)", Math.PI / 2.0, "(0 1 0)", "(1 0 0)")]
	[InlineData("(0 0 1)", Math.PI / 2.0, "(-1 0 0)", "(0 1 0)")]
	[InlineData("(0 0 1)", Math.PI / 2.0, "(0 -1 0)", "(-1 0 0)")]
	// Skewb rotations (120 degree corners)
	[InlineData("(1 1 1)", Math.PI * 2.0 / 3.0, "(1 0 0)", "(0 0 1)")]
	[InlineData("(1 1 1)", -Math.PI * 2.0 / 3.0, "(1 0 0)", "(0 1 0)")]
	public void RotateAndGetFace_FindFace(string rotationAxisCc, double theta, string faceCc, string expectedCc)
	{
		// 1. Prepare
		Cartesian3dCoordinate axis = new Cartesian3dCoordinate(rotationAxisCc);
		Cartesian3dCoordinate faceOrientation = new Cartesian3dCoordinate(faceCc);
		Cartesian3dCoordinate expectedOrientation = new Cartesian3dCoordinate(expectedCc);
		Block b = new(new BlockDefinition("id", Cartesian3dCoordinate.XAxis, new BlockFace("test", faceOrientation)));

		// 2. Execute
		b.RotateAround(axis, theta);
		BlockFace f = b.GetBlockFace(expectedOrientation);

		// 3. Verify
		Assert.NotNull(f);
	}

	[Fact]
	public void CreateAndGetFaceWithBadOrientation_ReturnNull()
	{
		// 1. Prepare
		SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);
		Block b = new(new BlockDefinition("id", Cartesian3dCoordinate.XAxis, new BlockFace("test", faceOrientation)));

		// 2. Execute
		BlockFace f = b.GetBlockFace(new SphericalVector(100, 100));

		// 3. Verify
		Assert.Null(f);
	}

	[Theory]
	[InlineData("(0 0 1)", "(1 0 0)", Math.PI / 2.0)]
	public void CreateRotateAndGetFaceWithOriginalOrientation_ReturnNull(string axisCc, string originalFace, double theta)
	{
		// 1. Prepare
		Cartesian3dCoordinate axis = new Cartesian3dCoordinate(axisCc);
		Cartesian3dCoordinate faceOrientation = new Cartesian3dCoordinate(originalFace);
		Block b = new(new BlockDefinition("id", Cartesian3dCoordinate.XAxis, new BlockFace("test", faceOrientation)));

		b.RotateAround(axis, theta);

		// 2. Execute
		BlockFace f = b.GetBlockFace(faceOrientation);

		// 3. Verify
		Assert.Null(f);
	}

	#endregion Test Methods
}