﻿using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Tests.DataFactories;
using Twisty.Engine.Structure.Topology;

namespace Twisty.Engine.Tests.Structure;

[Trait("Category", "Structure/Topology")]
public class AngularTopologyBuilderTest
{
	#region Test Methods

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
		var b = new Block("id", axis, BlockFacesFactory.GetFaces(facesId));
		var builder = new AngularTopologyBuilder();

		// 2. Execute
		var id = builder.GetTopologicId(b);

		// 3. Verify
		Assert.NotNull(id);
		Assert.Equal(expected, id);
	}

	[Theory]
	// Single Face
	[InlineData("(1 0 0)", "Y", "(0 0 1)", Math.PI)]
	[InlineData("(1 0 0)", "Z", "(0 0 1)", Math.PI)]
	[InlineData("(0 0 1)", "X", "(0 1 1)", Math.PI)]
	[InlineData("(0 0 1)", "Y", "(0 1 1)", Math.PI)]
	[InlineData("(0 1 0)", "Y", "(0 0 1)", Math.PI)]
	[InlineData("(1 1 1)", "Y", "(0 0 1)", Math.PI)]
	[InlineData("(1 1 1)", "-Y", "(0 0 1)", Math.PI / 3.0)]
	[InlineData("(-1 -1 -1)", "Y", "(0 0 1)", Math.PI / 2.0)]
	[InlineData("(-1 -1 -1)", "-Y", "(0 0 1)", Math.PI * 2.0)]
	// Two Faces
	[InlineData("(1 0 0)", "Y,Z", "(0 0 1)", Math.PI)]
	[InlineData("(1 0 0)", "Z,Y", "(0 0 1)", Math.PI)]
	[InlineData("(1 0 0)", "X,Y", "(0 0 1)", Math.PI)]
	[InlineData("(1 0 0)", "Y,X", "(0 0 1)", Math.PI)]
	[InlineData("(1 0 0)", "-X,Y", "(0 0 1)", Math.PI * 0.9)]
	[InlineData("(1 0 0)", "-Y,X", "(0 0 1)", Math.PI * 5.0)]
	// Complex
	[InlineData("(0 0 1)", "X,AlmostX", "(0 0 1)", Math.PI * 5.0)]
	[InlineData("(0 0 1)", "AlmostX,X", "(0 0 1)", Math.PI * 0.8)]
	[InlineData("(0 0 1)", "-X,AlmostX", "(0 0 1)", Math.PI * 0.7)]
	[InlineData("(0 0 1)", "AlmostX,-X", "(0 0 1)", Math.PI * 0.5)]
	[InlineData("(0 0 1)", "-X,AlmostX,Y", "(8 7 1)", Math.PI * 0.4)]
	[InlineData("(0 0 1)", "AlmostX,-X,Y", "(13 -4 1)", Math.PI * 0.3)]
	[InlineData("(0 0 1)", "Y,AlmostX,-X", "(20 100 15)", Math.PI * 0.2)]
	public void GetRotatedBlockTopologicId_ReturnExpected(string axisCc, string facesId, string rotationAxisCc, double rotationTheta)
	{
		// 1. Prepare
		var axis = new Cartesian3dCoordinate(axisCc);
		var rotationAxis = new Cartesian3dCoordinate(rotationAxisCc);
		var b = new Block("id", axis, BlockFacesFactory.GetFaces(facesId));
		var builder = new AngularTopologyBuilder();

		// Keep original id as rotated one should stay the same.
		var id = builder.GetTopologicId(b);
		b.RotateAround(rotationAxis, rotationTheta);

		// 2. Execute
		var rotatedId = builder.GetTopologicId(b);

		// 3. Verify
		Assert.NotNull(id);
		Assert.NotNull(rotatedId);
		Assert.Equal(id, rotatedId);
	}

	#endregion Test Methods
}