using System;
using System.Linq;
using System.Collections.Generic;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Tests.Structure;

[Trait("Category", "Structure")]
public class BlockTest
{
	#region Test Methods

	[Fact]
	public void CreateBlockWithNullId_ThrowArgumentNullException()
	{
		// 1. Prepare
		Block b;
		var faces = GetFaces("X");

		// 2. Execute
		void a1() => b = new(null, Cartesian3dCoordinate.XAxis, faces[0]);
		void a2() => b = new(null, Cartesian3dCoordinate.XAxis, faces);

		// 3. Verify
		Assert.Throws<ArgumentNullException>(a1);
		Assert.Throws<ArgumentNullException>(a2);
	}

	[Theory]
	[InlineData("")]
	[InlineData("\t")]
	[InlineData(" ")]
	[InlineData("    ")]
	[InlineData("\n")]
	public void CreateBlockWithEmptyId_ThrowArgumentNullException(string id)
	{
		// 1. Prepare
		Block b;
		var faces = GetFaces("X");

		// 2. Execute
		void a1() => b = new(id, Cartesian3dCoordinate.XAxis, faces[0]);
		void a2() => b = new(id, Cartesian3dCoordinate.XAxis, faces);

		// 3. Verify
		Assert.Throws<ArgumentException>(a1);
		Assert.Throws<ArgumentException>(a2);
	}

	[Fact]
	public void CreateBlockWithNullFace_ThrowArgumentNullException()
	{
		// 1. Prepare
		Block b;
		BlockFace f = null;
		BlockFace[] faces = null;

		// 2. Execute
		void a1() => b = new("id", Cartesian3dCoordinate.XAxis, f);
		void a2() => b = new("id", Cartesian3dCoordinate.XAxis, faces);

		// 3. Verify
		Assert.Throws<ArgumentNullException>(a1);
		Assert.Throws<ArgumentNullException>(a2);
	}

	[Fact]
	public void CreateBlockWithEmptyFaces_ThrowArgumentException()
	{
		// 1. Prepare
		Block b;
		BlockFace[] faces = [];

		// 2. Execute
		void a() => b = new("id", Cartesian3dCoordinate.XAxis, faces);

		// 3. Verify
		Assert.Throws<ArgumentException>(a);
	}

	[Fact]
	public void CreateAndGetFace_ShouldFindFace()
	{
		// 1. Prepare
		SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);

		// 2. Execute
		Block b = new("id", Cartesian3dCoordinate.XAxis, new BlockFace("test", faceOrientation));
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
		Block b = new("id", Cartesian3dCoordinate.XAxis, new BlockFace("test", faceOrientation));

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
		Block b = new("id", Cartesian3dCoordinate.XAxis, new BlockFace("test", faceOrientation));

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
		Block b = new("id", Cartesian3dCoordinate.XAxis, new BlockFace("test", faceOrientation));

		b.RotateAround(axis, theta);

		// 2. Execute
		BlockFace f = b.GetBlockFace(faceOrientation);

		// 3. Verify
		Assert.Null(f);
	}

	[Theory]
	// Single Face
	[InlineData("(1 0 0)", "Y", "0")]
	[InlineData("(1 0 0)", "Z", "0")]
	[InlineData("(0 0 1)", "X", "0")]
	[InlineData("(0 0 1)", "Y", "0")]
	[InlineData("(0 1 0)", "Y", "157")]
	[InlineData("(1 1 1)", "Y", "62")]
	[InlineData("(1 1 1)", "-Y", "62")]
	[InlineData("(-1 -1 -1)", "Y", "62")]
	[InlineData("(-1 -1 -1)", "-Y", "62")]
	// Two Faces
	[InlineData("(1 0 0)", "Y,Z", "0-157-0")]
	[InlineData("(1 0 0)", "Z,Y", "0-157-0")]
	[InlineData("(1 0 0)", "X,Y", "0-157-157")]
	[InlineData("(1 0 0)", "Y,X", "0-157-157")]
	[InlineData("(1 0 0)", "-X,Y", "0-157-157")]
	[InlineData("(1 0 0)", "-Y,X", "0-157-157")]
	// Axed Corners
	[InlineData("(1 1 1)", "X,Y,Z", "62-157-62-157-62")]
	[InlineData("(-1 -1 -1)", "-X,-Y,-Z", "62-157-62-157-62")]
	[InlineData("(-1 1 1)", "-X,Y,Z", "62-157-62-157-62")]
	// Unaxed Corners
	[InlineData("(1 1 2)", "X,Y,Z", "42-157-96-157-42")]
	[InlineData("(1 1 2)", "Y,Z,X", "42-157-96-157-42")]
	[InlineData("(1 1 2)", "Z,X,Y", "42-157-96-157-42")]
	[InlineData("(1 1 2)", "Z,Y,X", "42-157-96-157-42")]
	[InlineData("(-1 -2 -1)", "-X,-Y,-Z", "42-157-96-157-42")]
	[InlineData("(-1 9 1)", "-X,Y,Z", "11-157-11-157-141")]
	[InlineData("(-1 9 1)", "X,-Y,Z", "11-157-11-157-141")]
	[InlineData("(-1 9 1)", "X,Y,-Z", "11-157-11-157-141")]
	// Complex
	[InlineData("(0 0 1)", "X,AlmostX", "0-14-10")]
	[InlineData("(0 0 1)", "AlmostX,X", "0-14-10")]
	[InlineData("(0 0 1)", "-X,AlmostX", "0-300-10")]
	[InlineData("(0 0 1)", "AlmostX,-X", "0-300-10")]
	[InlineData("(0 0 1)", "-X,AlmostX,Y", "0-157-0-300-10")]
	[InlineData("(0 0 1)", "AlmostX,-X,Y", "0-157-0-300-10")]
	[InlineData("(0 0 1)", "Y,AlmostX,-X", "0-157-0-300-10")]
	public void GetSimpleBlockTopologicId_ReturnExpected(string axisCc, string facesId, string expected)
	{
		// 1. Prepare
		var axis = new Cartesian3dCoordinate(axisCc);
		var b = new Block("id", axis, GetFaces(facesId));

		// 2. Execute
		var id = b.TopologicId;

		// 3. Verify
		Assert.NotNull(id);
		Assert.Equal(expected, id);
	}

	#endregion Test Methods

	private static BlockFace[] GetFaces(string facesId)
		=> facesId.Split(',').Select(id => id switch
		{
			"X" => new BlockFace("X", Cartesian3dCoordinate.XAxis),
			"Y" => new BlockFace("Y", Cartesian3dCoordinate.YAxis),
			"Z" => new BlockFace("Z", Cartesian3dCoordinate.ZAxis),
			"-X" => new BlockFace("-X", Cartesian3dCoordinate.XAxis.Reverse),
			"-Y" => new BlockFace("-Y", Cartesian3dCoordinate.YAxis.Reverse),
			"-Z" => new BlockFace("-Z", Cartesian3dCoordinate.ZAxis.Reverse),
			"AlmostX" => new BlockFace("AlmostX", new Cartesian3dCoordinate(1, 0.1, 0.1)),
			"C1" => new BlockFace("C1", new Cartesian3dCoordinate(1, 1, 1)),
			"C2" => new BlockFace("C2", new Cartesian3dCoordinate(-1, -1, -1)),
			_ => throw new NotImplementedException()
		}).ToArray();
}